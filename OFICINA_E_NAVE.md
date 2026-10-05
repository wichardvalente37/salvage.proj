# Oficina e nave organizada

A construção de EchoLocator, EchoSensor e SoundBait só funciona na cena ShipInterior, junto da bancada da oficina. Isso vale para os botões e para as teclas 1/2/3. Fora da bancada, a construção é recusada sem gastar sucata. Uma parede entre o jogador e a bancada também impede a interação.

Entra pela nave em (26, 9) na MainGame, usando E. No interior, vai para o compartimento superior esquerdo: a entrada fica em (-4, 1), e a bancada em (-4, 4.3). Fica junto da frente da bancada, por exemplo em (-4, 2.6), e abre a oficina com I. O alcance é 2.25 unidades. Instalar os dispositivos continua a exigir aproximação aos satélites no espaço.

O interior mantém o casco circular e usa tons neutros: contorno duplo, corredor central, oficina delimitada por paredes com entrada aberta, bancada com três bandejas e quatro caixas alinhadas no depósito. Paredes, bancada e caixas têm colisão. Pisos, juntas e acabamentos são apenas visuais.

O exterior mantém a nave arredondada, com casco, janela circular, escotilha e dois módulos laterais simétricos. As formas usam o sprite de círculo básico já existente no projeto e a textura quadrada simples Panel; não foram adicionadas imagens geradas. O astronauta mantém os sprites anteriores.

## Arquivos

- Criados `Assets/Scripts/Dispositivos/CraftingBench.cs` e `.meta`: restringem a interação à bancada da nave e verificam alcance e parede entre ela e o jogador.
- Alterado `Assets/Scripts/Player/DeviceWorkshop.cs`: verifica a disponibilidade da bancada antes de fabricar, para todos os métodos e atalhos.
- Alterado `Assets/Scripts/Player/WorkshopHud.cs`: explica onde fabricar e desativa a construção fora da bancada.
- Alterados `Assets/Scenes/ShipInterior.unity` e `Assets/Scenes/MainGame.unity`: interior organizado e exterior com formas básicas, incluindo colliders da oficina e bancada.
- Alterado `Assets/Tests/PlayMode/DevicesAndRunsTests.cs`: teste de bloqueio de construção no espaço, longe da bancada e através de parede, e sucesso junto da bancada.
- Criado `OFICINA_E_NAVE.md`: este registro; documentos anteriores descrevem as versões anteriores do cenário.

As verificações locais de sintaxe e cena passaram. O novo teste Play Mode foi adicionado, mas não executado: compilação e aparência no Unity ainda precisam de validação no editor.
