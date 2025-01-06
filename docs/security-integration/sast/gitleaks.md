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

Coming soon