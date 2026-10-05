# Interior da nave

**Layout atualizado:** a nave agora é circular e minimalista, com quatro caixas e cores neutras. O layout de compartimentos descrito abaixo é o histórico da primeira versão. Consulte [NAVE_MINIMALISTA.md](NAVE_MINIMALISTA.md) para o cenário atual.

## Cena e acesso

Abre `Assets/Scenes/ShipInterior.unity` no Unity 6000.4.3f1 para editar ou testar diretamente. Os objetos do cenário e seus colliders estão serializados na cena, visíveis no editor; não são construídos apenas em runtime.

Também podes entrar pela MainGame: a escotilha verde está perto da posição inicial do astronauta, em **(26, 9)**. Aproxima-te e pressiona **E**, ou clica no botão que aparece perto da escotilha. Dentro da nave, o astronauta começa em **(0, -4.5)**; a escotilha de saída fica em **(0, -7.2)**. Usa E perto dela para voltar à MainGame.

As duas cenas estão habilitadas nas configurações de build. A transição grava os satélites antes de descarregar a cena e mantém RunState, sucata e equipamentos. Ao regressar, os satélites recuperam os valores guardados. O jogador e o monstro da MainGame voltam aos spawns iniciais; não é um save de posição.

## Organização

- Esquerda: oficina, duas bancadas e armário.
- Centro superior: cabine de comando, consoles e assento.
- Direita: depósito com quatro caixas.
- Centro inferior: corredor de circulação e escotilha.
- Casco fechado, divisórias internas e passagens abertas entre os compartimentos.

São **20 BoxCollider2D sólidos** nas paredes e no mobiliário, mais o collider do jogador. Pisos, juntas e faixas luminosas são decoração e não bloqueiam o movimento. As portas têm vãos físicos entre as divisórias; a escotilha interage por distância e não usa trigger. Não há monstro dentro da nave.

A arte do ambiente é uma composição inicial de painéis metálicos em tons escuros, com faixas de iluminação ciano. O ZIP anterior não continha imagens do interior da nave. O cenário reutiliza o astronauta animado e uma textura branca simples para os painéis; podes trocar os sprites mantendo os colliders e posições.

## Controles

- **WASD:** andar na nave a 4 unidades por segundo.
- **Espaço + direção:** impulso curto.
- **E perto da escotilha:** entrar/sair.
- **I:** abrir/fechar a oficina compacta; construção e inventário funcionam dentro da nave.

O movimento no espaço mantém o comportamento anterior: `walkSpeed` tem valor padrão zero. A nave configura `walkSpeed = 4`, impulso menor, deteção de colisão contínua e rotação congelada no Rigidbody2D. A câmera mostra a planta inteira e ajusta o enquadramento à proporção da janela.

O botão N de recarregar a MainGame continua com a função anterior. Dentro da nave, a saída é pela escotilha; não há um RunTransition para recarregar a nave através desse botão.

## Arquivos adicionados

| Arquivo | Finalidade |
| --- | --- |
| `Assets/Scenes/ShipInterior.unity` e `.meta` | Nova cena completa, com piso, paredes, móveis, colliders, astronauta, câmera, luz global e escotilha. |
| `Assets/Scripts/GameManager/ScenePortal.cs` e `.meta` | Interação por proximidade/E, validação da cena de destino e conservação do estado antes de viajar. |
| `Assets/Scripts/GameManager/ShipSceneController.cs` e `.meta` | Inicializa estado e oficina, ajusta a câmera e mostra nomes dos compartimentos. |
| `Assets/Sprites/Ship/Panel.png` e `.meta` | Textura branca de 16 × 16 para colorir pisos, paredes e mobiliário. |
| `Assets/Sprites/Ship.meta` | Identidade da pasta de arte. |
| `CENARIO_NAVE.md` | Este registro do cenário, controles e validação. |

## Arquivos alterados

| Arquivo | Alteração |
| --- | --- |
| `Assets/Scenes/MainGame.unity` | Escotilha de entrada em (26, 9) e sua faixa luminosa. |
| `ProjectSettings/EditorBuildSettings.asset` | ShipInterior adicionada, preservando MainGame na primeira posição. |
| `Assets/Scripts/Player/PlayerMovement.cs` | Caminhada opcional, com valor padrão zero para preservar o espaço; proteção quando não há teclado. |
| `Assets/Tests/PlayMode/DevicesAndRunsTests.cs` | Teste de ida/volta, preservação de valores, parede bloqueante e corredores abertos; limpeza também da cena da nave. |

## Validação

Passaram as verificações locais de YAML, IDs únicos, referências internas das cenas, cadastro no build, sintaxe C# e `git diff --check`. O editor Unity não está disponível aqui: a cena ainda precisa de importação, compilação e teste visual/físico em Play Mode.

O novo teste está na suite `DevicesAndRunsTests`, em **Window → General → Test Runner → PlayMode**. Foi adicionado, mas não executado neste ambiente.

Para testar manualmente: entra na nave pela MainGame; tenta atravessar paredes e caixas; percorre as passagens da oficina, depósito e cabine; constrói um dispositivo na oficina; regressa à MainGame e confirma o inventário e a vida/dispositivos dos satélites. Repete numa janela Game estreita para verificar o enquadramento.

## Correção do erro CS0117

Em `Assets/Scripts/GameManager/ScenePortal.cs`, a verificação de disponibilidade da cena foi corrigida de `SceneManager.CanStreamedLevelBeLoaded` para `Application.CanStreamedLevelBeLoaded`, que é a classe que fornece essa API no Unity. O carregamento continua a usar `SceneManager.LoadSceneAsync`. Compilação no editor ainda depende de validação no Unity.
