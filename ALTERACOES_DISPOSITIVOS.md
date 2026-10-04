# Sucata, dispositivos e continuidade das runs

## Como jogar

A cena `Assets/Scenes/MainGame.unity` já contém três pickups amarelos de sucata perto da posição inicial do jogador. Cada pickup dá 8 unidades ao encostar nele e desaparece. O inventário aparece no canto superior esquerdo.

| Ação | Tecla | Custo de sucata |
| --- | --- | --- |
| Construir EchoLocator | 1 | 5 |
| Construir EchoSensor | 2 | 8 |
| Construir SoundBait | 3 | 3 |
| Instalar EchoLocator no satélite mais próximo | F | — |
| Instalar EchoSensor no satélite mais próximo | G | — |
| Instalar SoundBait no satélite mais próximo | H | — |

Também existem botões para construir no painel. Construir retira sucata e acrescenta um dispositivo ao inventário; instalar consome uma unidade desse dispositivo. A construção pode acontecer em qualquer posição. A instalação exige estar a até 3 unidades do centro do satélite. Não gasta dispositivos se estiver longe, sem inventário ou se esse equipamento já estiver instalado. Se dois satélites estiverem no alcance, o alvo é o mais próximo.

- EchoLocator ativa apenas `hasEchoLocator`.
- EchoSensor ativa `hasEchoLocator` e `hasSensor`. Pode melhorar um satélite que já tenha EchoLocator, consumindo um EchoSensor.
- SoundBait ativa apenas `hasSoundBait`. Agora pode ser ativado com B junto do satélite, ou clique direito na CamCam se o satélite também tiver EchoLocator.
- Instalar um dispositivo preserva os outros dispositivos e a vida do satélite.
- E continua a reparar; Tab continua a alternar a CamCam. A lógica existente dos sensores e do sonar foi preservada.

Os custos e o alcance são campos públicos de `DeviceWorkshop`. Para configurar pelo Inspector, adiciona esse componente ao Player e ajusta os campos; se não existir, `SateliteManager` adiciona-o automaticamente com os valores acima. Os métodos públicos de construção e instalação podem ser chamados por outra interface. Os campos de contagem de `Machines` agora refletem o inventário persistente; alterar esses campos não concede equipamentos.

## Persistência

`RunState` é um GameObject criado automaticamente pelo manager. O seu `Start()` chama `DontDestroyOnLoad(gameObject)`. Existe apenas uma instância; duplicados são destruídos. Guarda sucata, dispositivos ainda no inventário e, por `Satelite.Name`, a vida e os três bools de cada satélite. Não guarda referências aos objetos da cena.

Ao carregar uma cena, o manager restaura os satélites conhecidos. O dano aleatório inicial antigo só se aplica a satélites ainda sem estado salvo. As reparações e instalações guardam imediatamente; `Satelite.OnDisable()` guarda também o estado ao desativar o objeto ou descarregar a cena.

**O campo `Name` deve ser único na cena e permanecer igual para o mesmo satélite nas próximas runs.** A cena principal tinha quatro instâncias com o nome herdado F19; agora têm F19, F20, F21 e F22. O manager escreve erro no Console se houver nomes vazios ou repetidos. Para novas cenas, usa esses mesmos identificadores apenas quando representam os mesmos satélites, inclui `SateliteManager` e um Player com a tag `Player`.

A continuidade é em memória durante transições de cenas e recarregamentos da cena, enquanto `RunState` existir. Sair do jogo/Play Mode reinicia o estado. Não foi adicionado save em disco. Pickups reaparecem ao recarregar a cena: a continuidade pedida aplica-se ao inventário e aos satélites, não aos objetos de sucata já recolhidos.

## Arquivos adicionados

| Arquivo | O que foi adicionado e para quê |
| --- | --- |
| `Assets/Scripts/Persistence/RunState.cs` | Singleton persistente, inventário de sucata/dispositivos, operações de construção/consumo e estados dos satélites indexados por nome. Também define `DeviceType`. |
| `Assets/Scripts/Player/DeviceWorkshop.cs` | Construção, instalação com limite de distância, controles de teclado, métodos públicos para UI e painel de inventário/construção. |
| `Assets/Scripts/Dispositivos/ScrapPickup.cs` | Recolha de sucata por trigger do jogador, com proteção contra recolha duplicada. |
| `Assets/Prefabs/ScrapPickup.prefab` | Pickup reutilizável amarelo, com SpriteRenderer, CircleCollider2D trigger e ScrapPickup; quantidade inicial de 8. |
| `Assets/Scripts/Persistence.meta` | Identidade Unity da nova pasta. |
| `Assets/Scripts/Persistence/RunState.cs.meta` | Identidade Unity do script persistente. |
| `Assets/Scripts/Player/DeviceWorkshop.cs.meta` | Identidade Unity do script do jogador. |
| `Assets/Scripts/Dispositivos/ScrapPickup.cs.meta` | Identidade Unity do script de recolha. |
| `Assets/Prefabs/ScrapPickup.prefab.meta` | Identidade Unity do novo prefab. |
| `ALTERACOES_DISPOSITIVOS.md` | Este registro dos arquivos, comportamento, configuração e validação. |

## Arquivos existentes alterados

| Arquivo | O que mudou e porquê |
| --- | --- |
| `Assets/Scripts/GameManager/SateliteManager.cs` | Cria/reutiliza RunState, liga DeviceWorkshop ao jogador, valida nomes e restaura estados. Evita sortear novamente a vida de satélites já conhecidos. |
| `Assets/Scripts/Satelite/Satelite.cs` | Adicionado OnDisable para guardar a vida e os dispositivos antes de perder o objeto da cena. |
| `Assets/Scripts/Satelite/SateliteHealth.cs` | Repair guarda o novo estado imediatamente depois de atualizar a vida. |
| `Assets/Scripts/Dispositivos/Machines.cs` | Atualiza as três contagens existentes a partir do inventário persistente, mantendo a lógica do sonar. |
| `Assets/Prefabs/Satelite .prefab` | hasSensor passa de true para false. Os três dispositivos ficam desligados por padrão, como solicitado. |
| `Assets/Scenes/MainGame.unity` | Identificações F19–F22 e três pickups de 8 sucatas nas posições (21, 8.5), (18, 6) e (14, 5). Os pickups da cena são objetos independentes, com os mesmos componentes do novo prefab. |

## Configuração de sucata adicional

Arrasta `Assets/Prefabs/ScrapPickup.prefab` para a cena e ajusta `amount`. O collider deve continuar como trigger. O jogador precisa de collider 2D e Rigidbody2D para os eventos físicos; a cena principal já os tem. Os pickups reconhecem o componente `DeviceWorkshop` no jogador ou no pai do collider.

A primeira alteração construía/instalava SoundBait e ativava o bool solicitado. A atualização seguinte implementa a ativação: B perto do satélite ou clique direito na CamCam; ver `SPRITES_E_LOGICA.md`.

## Validação

Executado neste ambiente:

- Parsing YAML da cena principal e dos dois prefabs afetados: passou.
- Verificação de IDs de objetos sem duplicações, identificadores F19–F22 e três pickups na cena: passou.
- Verificação das referências dos pickups aos scripts e aos componentes: passou.
- `git diff --check`: passou.

**Compilação e Play Mode não executados:** o editor Unity 6000.4.3f1 não está instalado neste ambiente. As verificações de estrutura acima não substituem a importação e execução no Unity.

### Roteiro de teste no Unity

1. Abre MainGame e entra em Play Mode. Confirma o painel, os três pickups e ausência de erros de compilação/Console.
2. Sem sucata, tenta construir: o inventário não deve mudar. Recolhe um pickup: deve dar exatamente 8 sucatas e desaparecer.
3. Constrói EchoLocator: deve gastar 5 e dar uma unidade. Longe do satélite, F não deve consumi-lo. Perto, F deve consumi-lo e ativar apenas hasEchoLocator. Repetir a instalação não deve consumir outra unidade.
4. Recolhe mais sucata, constrói EchoSensor e instala com G. Confirma os dois bools ativos, incluindo a melhoria de um satélite já com EchoLocator. Tab e sonar devem continuar a funcionar como antes.
5. Constrói e instala SoundBait com H. Confirma hasSoundBait ativo e os outros valores preservados.
6. Repara com E e anota a vida. Recarrega MainGame usando SceneManager.LoadScene, ou transita para outra cena configurada com o mesmo Name. Confirma vida, bools, sucata e inventário preservados, sem novo dano aleatório sobre o satélite conhecido e apenas um RunState.
7. Sai e volta a Play Mode: deve começar uma sessão nova. Testa custos/alcance personalizados e dois colliders do jogador para confirmar que cada pickup é recolhido só uma vez.

## Atualização: sprites e lógica completada

Ver [SPRITES_E_LOGICA.md](SPRITES_E_LOGICA.md) para a importação dos 20 sprites, animações, isca sonora, correções da CamCam, tecla N/botão de próxima run e testes Play Mode.

## Menu atualizado

O painel agora inicia recolhido e abre com **I**. Construção, instalação e próxima run estão nas abas Construir, Satélite e Run. Ver [MENU_COMPACTO.md](MENU_COMPACTO.md) para os arquivos alterados e validação.
