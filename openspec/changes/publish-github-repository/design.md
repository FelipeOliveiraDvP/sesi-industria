# Design

## Context

Consulte `proposal.md` para a motivação. Na exploração, o clone estava limpo, na branch `main`, sem remoto configurado; as branches locais `main` e `dev` apontavam para o mesmo commit. O GitHub autenticou a conta e aceitou o handshake de um `git push --dry-run`, mas nenhum ref remoto foi anunciado nem criado. Revalidar o estado remoto antes de publicar.

O workflow `.github/workflows/ci.yml` já executa frontend, backend e smoke tests em `push`, `pull_request` e execução manual, com `contents: read`; não contém deploy. O README da raiz tem placeholder de clone e texto dizendo que não há remoto. O guia Railway não fixa branch e informa que a associação e o deploy externo não foram feitos. Os READMEs das aplicações apontam para a raiz e para o guia.

A change `prepare-railway-demo-deployment` preparou arquivos locais e está completa; ela não realizou operações GitHub ou Railway. Esta change cobre a publicação no GitHub e o alinhamento da documentação, sem repetir a preparação já existente.

## Goals / Non-Goals

**Goals:**

- Publicar o estado versionado revisado no repositório informado e estabelecer `main` como branch padrão.
- Deixar as instruções de clone e a referência de branch consistentes com esse estado.
- Confirmar que a CI existente executa para `main` sem alterar sua política ou habilitar deploy.

**Non-Goals:**

- Publicar ou configurar serviços no Railway nesta change; a futura integração continua descrita como etapa separada.
- Alterar visibilidade, colaboradores, regras de proteção ou outras permissões do repositório.
- Adicionar workflow de deploy, token Railway, publicação de imagens ou novos serviços.
- Apagar ou renomear branches locais; `dev` permanece preservada, sem ser necessária para tornar `main` a principal.
- Alterar contratos, código da aplicação ou dependências.

## Decisions

### Repositório e branch principal

Configurar `origin` com a URL SSH fornecida e publicar a branch local `main`; verificar no GitHub que `main` é a branch padrão. Na exploração, o remoto não anunciou refs, mas isso deve ser confirmado novamente durante a aplicação.

Não usar `--force`, não sobrescrever refs existentes e não fazer merge/rebase automático caso o remoto tenha recebido commits desde a exploração. Se houver refs incompatíveis ou divergência, interromper a publicação e pedir uma decisão. A branch local `dev` não será apagada ou renomeada; publicar somente `main` mantém o escopo focado na branch principal solicitada.

**Alternativa:** publicar `dev` como branch principal preservaria o nome histórico, mas contraria a decisão explícita de usar `main`. Tornar `main` a default branch, sem reescrever o histórico, alinha os clones e a CI ao fluxo desejado.

### Documentação e revisão pré-publicação

Atualizar `README.md` com o endereço web do repositório e o clone SSH, substituir o placeholder e remover apenas a afirmação de que não há remoto. Atualizar `docs/deploy-railway.md` para referenciar o repositório e `main`, deixando explícito que serviços, domínios, variáveis, Wait for CI e deploy Railway continuam pendentes. Fazer uma busca final na documentação versionada para achar outros placeholders ou afirmações obsoletas; manter referências de terceiros e registros históricos do OpenSpec intactos.

Antes do push, revisar o conteúdo rastreado e as alterações para impedir a inclusão de `.env` local, chaves privadas, credenciais ou artefatos gerados. A chave SSH fica fora do repositório. A visibilidade e permissões existentes não serão modificadas.

Por decisão expressa do usuário, incluir `opencode.json` na publicação. O arquivo foi validado como JSON; a configuração é um endpoint HTTPS de MCP sem userinfo, query ou fragmento, e a varredura de marcadores de credenciais não encontrou correspondências. Repetir a revisão no diff final e interromper se o arquivo passar a conter credenciais literais.

**Alternativa:** espalhar instruções de clone por todos os READMEs duplicaria a fonte de verdade. Os READMEs das aplicações já direcionam para a raiz/guia; atualizar os pontos que contêm placeholders ou instruções de publicação mantém referências locais e não duplicadas.

### CI preservada; Railway adiado

Manter `.github/workflows/ci.yml` sem mudanças: seus eventos atuais já incluem push em `main`, pull requests e execução manual. Após publicar, verificar a execução real e os três jobs. Não adicionar deploy GitHub Actions nem secrets; a integração nativa Railway com **Wait for CI** continua sendo a opção documentada para uma etapa futura.

**Alternativa:** um job de deploy via token Railway exigiria credencial privilegiada e mudaria a arquitetura previamente preparada; não é necessário para publicar o código e manter a CI.

## Risks / Trade-offs

- **O remoto pode mudar antes da aplicação** → consultar refs e histórico imediatamente antes do push; parar diante de divergência, sem force push.
- **Segredos ou arquivos locais podem ser publicados** → revisar os arquivos rastreados, o diff e os resultados da varredura de segredos antes de criar/pushar o commit de documentação.
- **A CI pode falhar ou não iniciar** → conferir o run associado ao push de `main`; registrar a falha sem declarar aprovação e sem tentar contorná-la por deploy.
- **A branch padrão pode não ser definida automaticamente** → conferir a configuração do repositório após o primeiro push e ajustar somente a default branch para `main`, sem alterar acesso ou visibilidade.
- **A documentação pode sugerir deploy já realizado** → manter explícita a diferença entre arquivos Railway versionados e configuração/deploy externos ainda pendentes.

## Migration Plan

1. Confirmar árvore de trabalho limpa, branch `main`, URL do remoto, refs atuais e revisão de segurança dos arquivos versionados.
2. Atualizar e revisar a documentação; validar links, comandos de clone e consistência das afirmações sobre CI e Railway.
3. Configurar `origin` com a URL SSH fornecida e revisar o commit local que contém documentação, artifacts OpenSpec e `opencode.json`; publicar `main` sem reescrita de histórico. Se o remoto já não estiver vazio ou houver divergência, parar e reavaliar.
4. Confirmar `main` como branch padrão no GitHub e verificar a execução real da CI para o commit publicado.
5. Deixar a associação Railway, **Wait for CI**, serviços, banco, domínios, variáveis e deploy para uma change/operação futura.

Se o push falhar, não remover ou reescrever commits locais; corrigir a causa e repetir somente após verificar o estado remoto. Se o run de CI falhar, manter o resultado visível e investigar em uma etapa própria; não publicar no Railway como contorno. Reverter um erro de documentação por commit corretivo, sem force push.
