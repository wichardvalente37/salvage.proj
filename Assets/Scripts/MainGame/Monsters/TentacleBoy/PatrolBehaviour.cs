
using UnityEngine;

public class PatrolBehaviour : MonoBehaviour
{
    //Variáveis
    public float speed;
    //Esta variável é o numero de ticks necessario para o monstro parar de andar 
    //por um intervalo de tempo
    private int StopTicks;
    //Ticks é basicamente o numero de vezes que o monstro chegou no seu destino
    private int ticks;
    private int rand;
    private float cooldown;
    private float lastseeked;
    private GameObject[] Points;
    void Start()
    {
        Points = GameObject.FindGameObjectsWithTag("Point");
        rand = Random.Range(0, Points.Length);
        StopTicks = Random.Range(0, 6);
        cooldown = Random.Range(0, 11);
        lastseeked = -cooldown;
    }

   
    public void canSeek()
    {
        if(Time.time < cooldown + lastseeked)
        {
            return;
        }

        Seek();
        if(ticks >= StopTicks)
        {
            cooldown = Random.Range(0, 11);
            lastseeked = Time.time;
            StopTicks = Random.Range(0, 6);
            ticks = 0;
        }
    }

   void Seek()
    {
       
        
       

        if(Vector2.Distance(transform.position, Points[rand].transform.position)< 0.1f)
        {
            //Quando o monstro chegar na posição do ponto alvo, randomiza o proximo ponto
            //e aumenta os ticks
            rand = Random.Range(0, Points.Length);
            ticks++;
            Debug.Log("Numero de toques= " + ticks);
        }
        else
        {
            //segue a cena
 transform.position = Vector2.MoveTowards(transform.position, Points[rand].transform.position, speed * Time.deltaTime);
        }
    }
}
