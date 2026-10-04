# Menu compacto

O painel inicia recolhido: mostra apenas a quantidade de sucata e o botão Oficina. Pressiona **I** ou clica no botão para abrir/fechar.

Ao abrir, as ações ficam separadas em três abas:

- **Construir:** quantidades no inventário, custos e botões desativados quando falta sucata.
- **Satélite:** mostra o nome e a vida do satélite mais próximo no alcance, com botões para instalar e ativar a isca. Sem satélite perto, mostra uma orientação.
- **Run:** botão para recomeçar a cena mantendo os valores persistentes.

O painel usa cores escuras com destaque em ciano e escala proporcional em janelas menores. Mede 270 × 42 no estado recolhido, ou 270 × 240 aberto, antes da escala. Nenhuma tela cheia ou pausa é aplicada. Todos os atalhos anteriores continuam funcionando com o menu fechado: 1/2/3, F/G/H, B, E, Tab e N.

## Arquivos

- Adicionados `Assets/Scripts/Player/WorkshopHud.cs` e `.meta`: apresentação, abas, escala, abertura com I e área real de captura do mouse.
- Alterado `Assets/Scripts/Player/DeviceWorkshop.cs`: retirado o painel antigo; mantém a lógica e os atalhos.
- Alterado `Assets/Scripts/GameManager/RunTransition.cs`: retirado o botão independente que ficava sempre visível; a ação passou para a aba Run.
- Alterado `Assets/Scripts/GameManager/SateliteManager.cs`: adiciona WorkshopHud automaticamente ao jogador.
- Alterado `Assets/Scripts/Dispositivos/Machines.cs`: bloqueia cliques na área atual do painel, em vez do antigo retângulo fixo. Quando recolhido, o espaço abaixo fica disponível para interagir com a CamCam.
- Adicionado `MENU_COMPACTO.md`: este registro.
- Atualizados `SPRITES_E_LOGICA.md` e `ALTERACOES_DISPOSITIVOS.md`: apontam para os controles e organização atuais.

## Validação

Verificados sintaxe C# e `git diff --check`. Sem editor Unity disponível, a aparência e os cliques em Play Mode ainda precisam de validação.

No Unity: abre MainGame, confirma que só a barra pequena aparece; usa I, testa as três abas, volta a fechar e confirma os atalhos e cliques na CamCam fora do painel. Redimensiona a janela Game para confirmar a escala.
