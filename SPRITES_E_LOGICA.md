# Sprites, isca sonora e próxima run

## O que foi integrado

Os 20 PNGs de `Sprites.zip` foram copiados sem alterar os bytes para `Assets/Sprites/Imported`, preservando a estrutura de pastas. Foram adicionados `.meta` para importar cada PNG como um Sprite 2D individual, com transparência e pivot central.

- Astronauta parado: 6 frames, em loop a 8 fps.
- Astronauta a reparar: 4 frames, acionados por uma reparação válida com E. Depois volta à animação de parado. Satélites já totalmente reparados não produzem novo ruído nem animação.
- Monstro: 9 frames, em loop a 8 fps.
- Satélite: nova imagem no prefab, herdada pelas quatro instâncias da cena principal. O esticamento do sprite placeholder foi removido; o collider foi preservado.

As animações estão ligadas diretamente aos renderers da MainGame; não é necessário criar Animator Controllers. Para outras cenas, adiciona `SpriteFrameAnimation` ao objeto principal e liga `target`, `idleFrames` e, no astronauta, `repairFrames`. Os frames devem estar na ordem dos sufixos 0000, 0001, etc.

## Lógica completada

### Isca sonora

Fabricar com 3 e instalar com H continua a consumir um dispositivo e a ativar `hasSoundBait`.

Depois de instalada:

- **B**, até 3 unidades do centro do satélite mais próximo: ativa a isca localmente. Não precisa de EchoLocator para esta ativação.
- **Clique direito sobre o satélite na CamCam**: ativa a isca remotamente. Para aparecer e ser selecionável nessa vista, esse satélite precisa também de EchoLocator ou EchoSensor.

O dispositivo instalado é reutilizável: não consome sucata a cada ativação. Tem cooldown de 10 segundos por satélite e alcance de atração de 25 unidades, configuráveis no `SoundBaitDevice`. Dentro do alcance, dá 10 pontos de suspeita ao monstro e envia a posição do satélite para investigação. A perseguição direta ao jogador tem prioridade: se o monstro estiver a perseguir, a isca não o obriga a abandonar a perseguição.

Não foi adicionado um arquivo de áudio, porque o ZIP só contém imagens. O efeito de gameplay é um estímulo de ruído para a IA, com confirmação no Console.

O ruído de reparação agora também informa diretamente a posição onde nasceu, quando chega ao monstro. A investigação não depende de encontrar um objeto temporário com a tag SusPosition. Foi corrigida a propagação do ruído para usar velocidade multiplicada por deltaTime.

### Próxima run

**N** ou o botão **Próxima run** recarrega a cena atual, que deve constar no Build Profile. MainGame já está listada nas configurações de build do projeto.

Antes de carregar, guarda a vida e os três bools dos satélites. O mesmo RunState sobrevive, conservando também sucata e dispositivos por instalar. A vida dos satélites conhecidos não é novamente sorteada. Player, monstro e pickups voltam às posições/valores iniciais da cena; os pickups podem ser recolhidos novamente.

Esta transição serve para a continuidade entre runs pedida, reutilizando MainGame; não foi criado um segundo mapa. A persistência continua em memória, sem save em disco.

### CamCam

A alternância com Tab agora guarda e restaura a visibilidade anterior de cada renderer. Assim, os novos sprites dos satélites voltam a aparecer na vista normal. Foram removidos o limite de 20 marcadores e a dependência de clique segurado para o sonar. O marcador do astronauta acompanha o jogador, e satélites que recebem EchoLocator enquanto o mapa está aberto passam a ter marcador.

Clique esquerdo em um satélite equipado emite sonar, com o cooldown existente de 7,5 segundos na MainGame. Clique direito ativa a isca instalada. Os cliques nos botões do inventário não devem disparar estes dispositivos. Machines encontra a câmera mesmo se OpenCam já a tiver desativado no Start, evitando depender da ordem de inicialização.

## Controles e roteiro de teste manual

| Comando | Ação |
| --- | --- |
| WASD + Espaço | Direção e impulso, conforme o movimento original |
| Encostar no pickup amarelo | Receber 8 unidades de sucata |
| 1 / 2 / 3 | Fabricar EchoLocator / EchoSensor / SoundBait; custos 5 / 8 / 3 |
| F / G / H perto do satélite | Instalar o dispositivo correspondente |
| E perto do satélite | Reparar e reproduzir os frames de reparação |
| Tab | Alternar a CamCam |
| Clique esquerdo num satélite da CamCam | Sonar para revelar temporariamente o monstro ao detetá-lo |
| Clique direito num satélite da CamCam | Ativar a isca se instalada e disponível |
| B perto do satélite | Ativar a isca localmente |
| N ou botão Próxima run | Recarregar mantendo o estado persistente |

1. Abre MainGame no Unity 6000.4.3f1 e entra em Play. Confirma as animações e a imagem do satélite.
2. Recolhe sucata, aproxima-te de um satélite e usa E. Verifica a animação de reparação e o aumento da vida no Inspector.
3. Fabrica EchoLocator e SoundBait, instala com F e H. Com o monstro a patrulhar dentro do alcance, usa B: deve investigar o satélite. Uma segunda ativação imediata deve ser recusada pelo cooldown.
4. Usa Tab. Testa clique esquerdo para sonar e clique direito para isca. Volta com Tab e confirma que os sprites originais reaparecem.
5. Anota a vida, os bools e o inventário. Usa N. Confirma os mesmos valores após o carregamento e apenas um RunState na Hierarchy.

## Testes automatizados e verificações

Foi adicionada uma suite Play Mode com quatro testes:

- Construção sem material suficiente e desconto exato de sucata.
- Instalação de EchoSensor: distância, melhoria de EchoLocator e prevenção de consumo duplicado.
- Isca: instalação obrigatória, alcance e cooldown.
- Transição real de MainGame: conservação de vida, bools, inventário e singleton.

Para executar no Unity: **Window → General → Test Runner → PlayMode → Run All**, ou seleciona `DevicesAndRunsTests`. Os testes usam reflexão para chamar os scripts existentes sem mover o projeto para uma nova assembly runtime.

Esses quatro testes **não foram executados neste ambiente**, que não dispõe do editor Unity. Também não foi feita uma compilação Unity nem validação visual em Play Mode. As verificações locais passaram: integridade dos 20 PNGs, YAML/importadores, identidades, referências serializadas, quantidades de frames, parsing de sintaxe C# dos 22 scripts e `git diff --check`; não substituem os testes do editor.

## Arquivos criados ou alterados

### Código, cena e documentação

| Arquivo | Finalidade |
| --- | --- |
| `Assets/Prefabs/Satelite .prefab` | Nova imagem e escala visual uniforme. |
| `Assets/Scenes/MainGame.unity` | Sprites iniciais e componentes de animação com todos os frames ligados. |
| `Assets/Scripts/Dispositivos/Machines.cs` | Cliques esquerdo/direito para sonar/isca na CamCam; bloqueia cliques sobre o inventário. |
| `Assets/Scripts/GameManager/OpenCam.cs` | Restaura renderers, acompanha jogador e atualiza marcadores sem limite fixo. |
| `Assets/Scripts/GameManager/SateliteManager.cs` | Liga automaticamente RunTransition e SoundBaitDevice. |
| `Assets/Scripts/Misc/SoundObject.cs` | Ruído transmite a origem à investigação e usa velocidade por segundo. |
| `Assets/Scripts/Monster/InvestigateBehaviour.cs` | Recebe estímulos por HearSound e investiga a posição recebida. |
| `Assets/Scripts/Monster/MonsterBrain.cs` | Expõe estado de perseguição e dá prioridade à perseguição sobre investigação. |
| `Assets/Scripts/Persistence/RunState.cs` | Protege o singleton também no Awake, antes de uma possível transição imediata. |
| `Assets/Scripts/Player/DeviceWorkshop.cs` | Ativação local da isca com B e ajuda de controles no painel. |
| `Assets/Scripts/Player/RepairSatelite.cs` | Aciona animação de reparação e evita reparar um satélite já cheio. |
| `Assets/Scripts/Dispositivos/SoundBaitDevice.cs` | Novo dispositivo com alcance, suspeita e cooldown. |
| `Assets/Scripts/GameManager/RunTransition.cs` | Nova transição entre runs com gravação antes de carregar. |
| `Assets/Scripts/Visuals/SpriteFrameAnimation.cs` | Novo reprodutor de frames para idle e reparação. |
| `Assets/Tests/PlayMode/DevicesAndRunsTests.cs` | Quatro testes de construção, instalação, isca e persistência entre runs. |
| `Assets/Tests/PlayMode/Game.PlayMode.Tests.asmdef` | Assembly de testes Play Mode separada dos scripts do jogo. |
| `ALTERACOES_DISPOSITIVOS.md` | Atualizado com os novos comandos e referências à funcionalidade agora concluída. |
| `SPRITES_E_LOGICA.md` | Este registro detalhado da integração e dos testes. |

### Sprites importados

Cada imagem abaixo recebeu também um arquivo `.meta` com o mesmo caminho seguido de `.meta`, para manter a identidade e a configuração de importação no Unity.

- `Assets/Sprites/Imported/Astronauta/consertar/Austronauta_consertar-1791124885732_0000.png`
- `Assets/Sprites/Imported/Astronauta/consertar/Austronauta_consertar-1791124886035_0001.png`
- `Assets/Sprites/Imported/Astronauta/consertar/Austronauta_consertar-1791124886286_0002.png`
- `Assets/Sprites/Imported/Astronauta/consertar/Austronauta_consertar-1791124886554_0003.png`
- `Assets/Sprites/Imported/Astronauta/idle/AUSTRONAUTA_IDLE-1791123083807_0000.png`
- `Assets/Sprites/Imported/Astronauta/idle/AUSTRONAUTA_IDLE-1791123084150_0001.png`
- `Assets/Sprites/Imported/Astronauta/idle/AUSTRONAUTA_IDLE-1791123084414_0002.png`
- `Assets/Sprites/Imported/Astronauta/idle/AUSTRONAUTA_IDLE-1791123084673_0003.png`
- `Assets/Sprites/Imported/Astronauta/idle/AUSTRONAUTA_IDLE-1791123085013_0004.png`
- `Assets/Sprites/Imported/Astronauta/idle/AUSTRONAUTA_IDLE-1791123085269_0005.png`
- `Assets/Sprites/Imported/monster/MONSTER-1791122974151_0000.png`
- `Assets/Sprites/Imported/monster/MONSTER-1791122974633_0001.png`
- `Assets/Sprites/Imported/monster/MONSTER-1791122974898_0002.png`
- `Assets/Sprites/Imported/monster/MONSTER-1791122975113_0003.png`
- `Assets/Sprites/Imported/monster/MONSTER-1791122975353_0004.png`
- `Assets/Sprites/Imported/monster/MONSTER-1791122975603_0005.png`
- `Assets/Sprites/Imported/monster/MONSTER-1791122975849_0006.png`
- `Assets/Sprites/Imported/monster/MONSTER-1791122976098_0007.png`
- `Assets/Sprites/Imported/monster/MONSTER-1791122976346_0008.png`
- `Assets/Sprites/Imported/satelite-1791124843668_0000.png`

### Metadados adicionais

Os seguintes `.meta` foram criados para as novas pastas, scripts e assembly de testes:

- `Assets/Scripts/Dispositivos/SoundBaitDevice.cs.meta`
- `Assets/Scripts/GameManager/RunTransition.cs.meta`
- `Assets/Scripts/Visuals.meta`
- `Assets/Scripts/Visuals/SpriteFrameAnimation.cs.meta`
- `Assets/Sprites/Imported.meta`
- `Assets/Sprites/Imported/Astronauta.meta`
- `Assets/Sprites/Imported/Astronauta/consertar.meta`
- `Assets/Sprites/Imported/Astronauta/idle.meta`
- `Assets/Sprites/Imported/monster.meta`
- `Assets/Tests.meta`
- `Assets/Tests/PlayMode.meta`
- `Assets/Tests/PlayMode/DevicesAndRunsTests.cs.meta`
- `Assets/Tests/PlayMode/Game.PlayMode.Tests.asmdef.meta`

## Menu atualizado

O painel agora inicia recolhido e abre com **I**. Construção, instalação e próxima run estão nas abas Construir, Satélite e Run. Ver [MENU_COMPACTO.md](MENU_COMPACTO.md) para os arquivos alterados e validação.
