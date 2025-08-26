# Semgrep 
The [Semgrep](https://github.com/semgrep/semgrep) analyzer performs Static Application Security Testing (SAST) scanning on repositories. It supports 30+ languages.

We developed an [analyzer](https://github.com/califio/code-secure-semgrep) that wraps Semgrep to integrate with Code Secure.

### CI Environment Variables

| ENV                    | Description                                                                                                        |
|------------------------|--------------------------------------------------------------------------------------------------------------------|
| SEMGREP_RULES          | Semgrep rules config                                                                                               |
| SEMGREP_SEVERITY       | Semgrep severify filter (semgrep --severity). Option:  INFO, WARNING, ERROR                                        |
| SEMGREP_EXCLUDED_PATHS | Semgrep exclude path (semgrep --exclude)                                                                           |
| SEMGREP_APP_TOKEN      | Semgrep app token. See more [here](https://semgrep.dev/docs/semgrep-ci/ci-environment-variables#semgrep_app_token) |
| SEMGREP_PRO            | Scan with pro engine. Require SEMGREP_APP_TOKEN variable                                                           |
| SEMGREP_VERBOSE        | Semgrep verbose (semgrep --verbose)                                                                                |
| SEMGREP_OUTPUT         | Semgrep scan result output. Default semgrep.json                                                                   |
| PROJECT_PATH           | Project dir to scan                                                                                                |

### GitLab CI/CD

```yaml
semgrep-sast-scan:
 image: ghcr.io/califio/code-secure-semgrep:latest
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
    container: ghcr.io/califio/code-secure-semgrep:latest
    steps:
      - name: Checkout code
        uses: actions/checkout@v4
        with:
          fetch-depth: 0
      - name: Semgrep Scan
        run: /analyzer run
  
```