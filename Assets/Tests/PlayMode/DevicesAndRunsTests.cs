using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

// Reflection allows this test assembly to exercise the existing Assembly-CSharp
// without moving all project scripts into a new runtime assembly.
public class DevicesAndRunsTests
{
    private readonly List<GameObject> created = new();
    private static Type RuntimeType(string name) => Type.GetType(name + ", Assembly-CSharp", true);
    private static object Call(object target, string name, params object[] args) =>
        target.GetType().GetMethod(name, BindingFlags.Public | BindingFlags.Instance).Invoke(target, args);
    private static void Field(object target, string name, object value) => target.GetType().GetField(name).SetValue(target, value);
    private static T Field<T>(object target, string name) => (T)target.GetType().GetField(name).GetValue(target);
    private static T Property<T>(object target, string name) => (T)target.GetType().GetProperty(name).GetValue(target);
    private static object Device(string name) => Enum.Parse(RuntimeType("DeviceType"), name);
    private static Component First(string name) =>
        (Component)Object.FindObjectsByType(RuntimeType(name), FindObjectsSortMode.None)[0];

    private Component New(string type)
    {
        var go = new GameObject("Test " + type);
        created.Add(go);
        return go.AddComponent(RuntimeType(type));
    }

    private static Component State() => (Component)RuntimeType("RunState")
        .GetMethod("GetOrCreate", BindingFlags.Public | BindingFlags.Static).Invoke(null, null);

    [SetUp]
    public void Setup()
    {
        var old = RuntimeType("RunState").GetProperty("Instance").GetValue(null) as Component;
        if (old != null) Object.DestroyImmediate(old.gameObject);
    }

    [UnityTearDown]
    public IEnumerator Cleanup()
    {
        // Unload the gameplay scene before destroying state so its OnDisable hooks cannot overwrite it.
        var game = SceneManager.GetSceneByName("MainGame");
        if (!game.IsValid() || !game.isLoaded) game = SceneManager.GetSceneByName("ShipInterior");
        if (game.IsValid() && game.isLoaded)
        {
            var empty = SceneManager.CreateScene("RunTestsCleanup");
            SceneManager.SetActiveScene(empty);
            yield return SceneManager.UnloadSceneAsync(game);
        }
        foreach (var go in created) if (go != null) Object.DestroyImmediate(go);
        created.Clear();
        var state = RuntimeType("RunState").GetProperty("Instance").GetValue(null) as Component;
        if (state != null) Object.DestroyImmediate(state.gameObject);
        yield return null;
    }

    [Test]
    public void CraftRejectsInsufficientScrapAndChargesExactlyOnce()
    {
        var state = State();
        Assert.IsFalse((bool)Call(state, "Craft", Device("EchoLocator"), 5));
        Call(state, "AddScrap", 8);
        Assert.IsTrue((bool)Call(state, "Craft", Device("EchoLocator"), 5));
        Assert.AreEqual(3, Property<int>(state, "Scrap"));
        Assert.AreEqual(1, Property<int>(state, "EchoLocators"));
        Assert.IsFalse((bool)Call(state, "Craft", Device("EchoSensor"), 8));
        Assert.AreEqual(3, Property<int>(state, "Scrap"));
    }

    [Test]
    public void EchoSensorRequiresProximityAndUpgradesWithoutDuplicateConsumption()
    {
        var state = State();
        Call(state, "AddScrap", 16);
        Call(state, "Craft", Device("EchoSensor"), 8);
        Call(state, "Craft", Device("EchoSensor"), 8);
        var workshop = New("DeviceWorkshop");
        var satellite = New("Satelite");
        satellite.gameObject.AddComponent(RuntimeType("SateliteHealth"));
        Field(satellite, "Name", "TestSatellite");
        Field(satellite, "hasEchoLocator", true);
        satellite.transform.position = new Vector3(20, 0, 0);
        Assert.IsFalse((bool)Call(workshop, "TryInstall", satellite, Device("EchoSensor")));
        Assert.AreEqual(2, Property<int>(state, "EchoSensors"));
        satellite.transform.position = Vector3.zero;
        Assert.IsTrue((bool)Call(workshop, "TryInstall", satellite, Device("EchoSensor")));
        Assert.IsTrue(Field<bool>(satellite, "hasEchoLocator"));
        Assert.IsTrue(Field<bool>(satellite, "hasSensor"));
        Assert.IsFalse(Field<bool>(satellite, "hasSoundBait"));
        Assert.AreEqual(1, Property<int>(state, "EchoSensors"));
        Assert.IsFalse((bool)Call(workshop, "TryInstall", satellite, Device("EchoSensor")));
        Assert.AreEqual(1, Property<int>(state, "EchoSensors"));
    }

    [Test]
    public void BaitRequiresInstallationRespectsRangeAndHasCooldown()
    {
        var satellite = New("Satelite");
        var bait = satellite.gameObject.AddComponent(RuntimeType("SoundBaitDevice"));
        var near = New("InvestigateBehaviour");
        var far = New("InvestigateBehaviour");
        far.transform.position = new Vector3(100, 0, 0);
        Assert.IsFalse((bool)Call(bait, "Activate"));
        Field(satellite, "hasSoundBait", true);
        Assert.IsTrue((bool)Call(bait, "Activate"));
        Assert.IsTrue(Field<bool>(near, "isInvestigating"));
        Assert.AreEqual(10f, Field<float>(near, "SusAmount"));
        Assert.IsFalse(Field<bool>(far, "isInvestigating"));
        Assert.IsFalse((bool)Call(bait, "Activate"));
        Assert.AreEqual(10f, Field<float>(near, "SusAmount"));
    }

    [UnityTest]
    public IEnumerator NextRunPreservesHealthDevicesInventoryAndSingleton()
    {
        yield return SceneManager.LoadSceneAsync("MainGame");
        yield return null;
        var state = State();
        Call(state, "AddScrap", 13);
        Assert.IsTrue((bool)Call(state, "Craft", Device("EchoLocator"), 5));
        var satellite = First("Satelite");
        string name = Field<string>(satellite, "Name");
        var health = satellite.GetComponent(RuntimeType("SateliteHealth"));
        Field(health, "health", 37);
        Field(satellite, "hasEchoLocator", true);
        Field(satellite, "hasSensor", true);
        Field(satellite, "hasSoundBait", true);
        Call(First("RunTransition"), "NextRun");
        float deadline = Time.realtimeSinceStartup + 15f;
        while (satellite != null && Time.realtimeSinceStartup < deadline) yield return null;
        Assert.IsTrue(satellite == null, "Scene transition did not complete.");
        yield return null;
        Assert.AreSame(state, State());
        Assert.AreEqual(1, Object.FindObjectsByType(RuntimeType("RunState"), FindObjectsSortMode.None).Length);
        Assert.AreEqual(8, Property<int>(state, "Scrap"));
        Assert.AreEqual(1, Property<int>(state, "EchoLocators"));
        Component restored = null;
        foreach (Component candidate in Object.FindObjectsByType(RuntimeType("Satelite"), FindObjectsSortMode.None))
            if (Field<string>(candidate, "Name") == name) restored = candidate;
        Assert.IsNotNull(restored);
        Assert.AreEqual(37, Field<int>(restored.GetComponent(RuntimeType("SateliteHealth")), "health"));
        Assert.IsTrue(Field<bool>(restored, "hasEchoLocator"));
        Assert.IsTrue(Field<bool>(restored, "hasSensor"));
        Assert.IsTrue(Field<bool>(restored, "hasSoundBait"));
    }
    [UnityTest]
    public IEnumerator ShipHasCircularHullClearRoutesAndPreservesStateOnReturn()
    {
        yield return SceneManager.LoadSceneAsync("MainGame");
        yield return null;
        var state = State();
        Call(state, "AddScrap", 9);
        var satellite = First("Satelite");
        string name = Field<string>(satellite, "Name");
        Field(satellite.GetComponent(RuntimeType("SateliteHealth")), "health", 22);
        Field(satellite, "hasEchoLocator", true);
        var boarding = First("ScenePortal");
        Assert.IsFalse((bool)Call(boarding, "Travel"), "Cannot board from outside the hatch range.");
        GameObject.FindWithTag("Player").transform.position = boarding.transform.position;
        Assert.IsTrue((bool)Call(boarding, "Travel"));
        float deadline = Time.realtimeSinceStartup + 15f;
        while (SceneManager.GetActiveScene().name != "ShipInterior" && Time.realtimeSinceStartup < deadline) yield return null;
        Assert.AreEqual("ShipInterior", SceneManager.GetActiveScene().name);
        yield return null;
        Assert.AreSame(state, State());
        Assert.AreEqual(9, Property<int>(state, "Scrap"));
        Assert.AreEqual(4f, Field<float>(First("PlayerMovement"), "walkSpeed"));
        Physics2D.SyncTransforms();
        var size = new Vector2(0.8f, 0.8f);
        Assert.IsNotNull(Physics2D.BoxCast(new Vector2(0, -4.5f), size, 0, Vector2.left, 8, 1).collider, "Circular hull must block leaving the ship.");
        Assert.IsNull(Physics2D.BoxCast(new Vector2(0, -4.5f), size, 0, Vector2.up, 4.5f, 1).collider, "Central corridor must remain open.");
        Assert.IsNull(Physics2D.BoxCast(Vector2.zero, size, 0, Vector2.left, 8, 1).collider, "Left path between boxes must remain open.");
        Assert.IsNull(Physics2D.BoxCast(Vector2.zero, size, 0, Vector2.right, 8, 1).collider, "Right path between boxes must remain open.");
        var exit = First("ScenePortal");
        GameObject.FindWithTag("Player").transform.position = exit.transform.position;
        Assert.IsTrue((bool)Call(exit, "Travel"));
        deadline = Time.realtimeSinceStartup + 15f;
        while (SceneManager.GetActiveScene().name != "MainGame" && Time.realtimeSinceStartup < deadline) yield return null;
        Assert.AreEqual("MainGame", SceneManager.GetActiveScene().name);
        yield return null;
        Assert.AreSame(state, State());
        Assert.AreEqual(9, Property<int>(state, "Scrap"));
        Component restored = null;
        foreach (Component candidate in Object.FindObjectsByType(RuntimeType("Satelite"), FindObjectsSortMode.None))
            if (Field<string>(candidate, "Name") == name) restored = candidate;
        Assert.IsNotNull(restored);
        Assert.AreEqual(22, Field<int>(restored.GetComponent(RuntimeType("SateliteHealth")), "health"));
        Assert.IsTrue(Field<bool>(restored, "hasEchoLocator"));
    }

    [UnityTest]
    public IEnumerator CraftingRequiresShipWorkbenchAndCannotReachThroughWalls()
    {
        yield return SceneManager.LoadSceneAsync("MainGame");
        yield return null;
        var state = State();
        Call(state, "AddScrap", 20);
        Assert.IsFalse((bool)Call(First("DeviceWorkshop"), "CraftEchoLocator"));
        Assert.AreEqual(20, Property<int>(state, "Scrap"));
        yield return SceneManager.LoadSceneAsync("ShipInterior");
        yield return null;
        var workshop = First("DeviceWorkshop");
        Assert.IsFalse((bool)Call(workshop, "CraftEchoLocator"));
        workshop.transform.position = new Vector3(-4, 6.1f, 0);
        Physics2D.SyncTransforms();
        Assert.IsFalse((bool)Call(workshop, "CraftEchoLocator"), "Cannot craft through the workshop wall.");
        workshop.transform.position = new Vector3(-4, 2.6f, 0);
        Physics2D.SyncTransforms();
        Assert.IsTrue((bool)Call(workshop, "CraftEchoLocator"));
        Assert.AreEqual(15, Property<int>(state, "Scrap"));
        Assert.AreEqual(1, Property<int>(state, "EchoLocators"));
    }

}
