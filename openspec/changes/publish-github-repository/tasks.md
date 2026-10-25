# Tasks

## 1. Pré-publicação e segurança

- [x] 1.1 Revalidar árvore de trabalho, branch atual, URL/estado de `origin` e refs remotos; confirmar `main` como origem de publicação e interromper sem force push se houver divergência ou conteúdo remoto inesperado.
- [x] 1.2 Revisar arquivos rastreados e alterações documentais em busca de `.env` local, chaves privadas, credenciais e artefatos gerados; confirmar que nenhum segredo será incluído no commit nem no push.

## 2. Documentação do repositório

- [x] 2.1 Atualizar `README.md` com link web e clone SSH do repositório canônico, substituir `URL_DO_REPOSITORIO` e remover a afirmação obsoleta de ausência de remoto; validar o comando e preservar a indicação de que ainda não há URL de aplicação publicada.
- [x] 2.2 Atualizar `docs/deploy-railway.md` para apontar ao repositório canônico e `main`, distinguindo a publicação no GitHub da configuração Railway ainda pendente; revisar que serviços, variáveis, domínios, Wait for CI e deploy não sejam descritos como concluídos.
- [x] 2.3 Pesquisar a documentação versionada por placeholders de repositório e afirmações de ausência de remoto; corrigir apenas as instruções atuais afetadas, preservar histórico OpenSpec e referências GitHub de terceiros, e verificar links relativos.

## 3. Publicação da branch principal

- [ ] 3.1 Configurar `origin` com a URL SSH indicada e criar o commit das alterações documentais em `main`; verificar `git remote -v`, o diff do commit e uma árvore de trabalho limpa, sem alterar ou remover a branch local `dev`.
- [ ] 3.2 Publicar `main` sem reescrever histórico, somente após confirmar que o push é novo ou fast-forward; verificar com `git ls-remote` que a ref remota `main` corresponde ao commit local e parar para decisão se o remoto tiver divergido.
- [ ] 3.3 Confirmar no GitHub que `main` está definida como branch padrão; se necessário, ajustar somente essa configuração e verificar que visibilidade e permissões do repositório permaneceram inalteradas.

## 4. CI e verificação integrada

- [ ] 4.1 Verificar o run de GitHub Actions associado ao commit publicado em `main` e confirmar os jobs de frontend, backend e containers; registrar qualquer falha sem alterar a CI ou iniciar deploy como contorno.
- [ ] 4.2 Fazer uma validação final do clone documentado e do estado publicado: confirmar que `main` pode ser obtida pelo endereço SSH, que a documentação não contém placeholders/afirmações obsoletas e que Railway continua explicitamente pendente.
