# Gitleaks
The [Gitleaks](https://github.com/gitleaks/gitleaks) is a tool for detecting secrets like passwords, API keys, and tokens in git repos, files.

We developed an [gitleaks-analyzer](https://github.com/califio/code-secure-gitleaks) that wraps Gitleaks to integrate with Code Secure.

### GitLab CI/CD

```yaml
secret-detection:
  image: ghcr.io/califio/code-secure-gitleaks:latest
  stage: test
  rules:
    - if: $CI_PIPELINE_SOURCE == "web"
    - if: $CI_MERGE_REQUEST_IID
    - if: $CI_COMMIT_BRANCH == $CI_DEFAULT_BRANCH
    - if: $CI_COMMIT_TAG
  script:
    - /analyzer run
```

### GitHub Action

```yaml
name: Security Scan
on:
  pull_request:
  push:
    branches:
      - main
    tags:
      - '*'
env:
  CODE_SECURE_URL: ${{ vars.CODE_SECURE_URL }}
  CODE_SECURE_TOKEN: ${{ secrets.CODE_SECURE_TOKEN }}
  GITHUB_TOKEN: ${{ secrets.GIT_TOKEN }}
jobs:
  secret-scan:
    runs-on: ubuntu-latest
    container: ghcr.io/califio/code-secure-gitleaks:latest
    steps:
      - name: Checkout code
        uses: actions/checkout@v4
        with:
          fetch-depth: 0
      - name: Secret Scan
        run: /analyzer run
  
```