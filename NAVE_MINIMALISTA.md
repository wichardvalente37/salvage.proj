# Nave minimalista

A nave agora é um círculo simples em cinza, com quatro caixas quadradas no interior. Foram retirados compartimentos, bancadas, consoles, cadeiras, juntas do piso, placas e faixas luminosas.

- Interior circular: raio de 9 unidades.
- Contorno físico: EdgeCollider2D fechado com 64 segmentos, que bloqueia sair do círculo sem bloquear o espaço interior.
- Quatro caixas: tamanho 2 × 2, em (-4, 3), (4, 3), (-4, -2) e (4, -2), cada uma com BoxCollider2D sólido.
- Cores neutras em escala de cinza; nenhuma cor de destaque no cenário.
- A nave visível na MainGame também passou a ser um círculo cinza em (26, 9), sem faixa luminosa.

O astronauta, a oficina compacta e a interação com E continuam funcionando. A saída dentro da nave permanece em (0, -7.2), sem decoração; ao aproximar-te aparece o botão/indicação para sair. Inventário e valores dos satélites continuam preservados na transição.

## Arquivos alterados

- `Assets/Scenes/ShipInterior.unity`: cenário reduzido aos círculos, caixas, contorno físico e objetos funcionais.
- `Assets/Scenes/MainGame.unity`: representação da nave circular e neutra; retirada a faixa luminosa.
- `Assets/Scripts/GameManager/ShipSceneController.cs`: retirados os nomes dos compartimentos; câmera ajustada ao formato circular.
- `Assets/Tests/PlayMode/DevicesAndRunsTests.cs`: nomes e mensagens do teste adaptados ao casco circular e caminhos entre caixas.
- `CENARIO_NAVE.md`: aviso de que esta versão substitui o layout anterior.
- `NAVE_MINIMALISTA.md`: este registro.

As verificações de YAML, raízes da cena, contorno fechado, quantidade de colliders, cores neutras e sintaxe C# passaram. Compilação e teste visual/físico no Unity continuam pendentes.
