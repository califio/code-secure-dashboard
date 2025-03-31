using System.Security.Claims;
using CodeSecure.Api.CI.Model;
using CodeSecure.Api.CI.Service;
using CodeSecure.Application.Module.Finding;
using CodeSecure.Authentication;
using CodeSecure.Authentication.Jwt;
using CodeSecure.Core.Entity;
using CodeSecure.Core.Enum;
using Microsoft.AspNetCore.Identity;

namespace CodeSecure.Application;

public class InitDataService(
    JwtUserManager userManager,
    RoleManager<Roles> roleManager,
    ICiService ciService,
    ILogger<InitDataService> logger
)
{
    private readonly List<Claim> adminClaims =
    [
        new(PermissionType.Project, PermissionAction.Read),
        new(PermissionType.Project, PermissionAction.Update),
        new(PermissionType.Project, PermissionAction.Delete),
        new(PermissionType.User, PermissionAction.Create),
        new(PermissionType.User, PermissionAction.Read),
        new(PermissionType.User, PermissionAction.Update),
        new(PermissionType.User, PermissionAction.Delete),
        new(PermissionType.Role, PermissionAction.Create),
        new(PermissionType.Role, PermissionAction.Read),
        new(PermissionType.Role, PermissionAction.Update),
        new(PermissionType.Role, PermissionAction.Delete),
        new(PermissionType.Finding, PermissionAction.Read),
        new(PermissionType.Finding, PermissionAction.Update),
        new(PermissionType.Finding, PermissionAction.Delete),
        new(PermissionType.CiToken, PermissionAction.Read),
        new(PermissionType.CiToken, PermissionAction.Create),
        new(PermissionType.CiToken, PermissionAction.Delete),
        new(PermissionType.Config, PermissionAction.Read),
        new(PermissionType.Config, PermissionAction.Update),
        new(PermissionType.Rule, PermissionAction.Create),
        new(PermissionType.Rule, PermissionAction.Update),
        new(PermissionType.Rule, PermissionAction.Delete),
    ];

    private readonly List<Claim> userClaims = [];

    public async Task InitDataAsync(bool isDevelopment)
    {
        //await context.Database.MigrateAsync();
        await CreateDefaultRolesAsync();
        await CreateDefaultSystemUserAsync();
        if (isDevelopment) await CreateSampleData();
    }

    private async Task CreateDefaultRolesAsync()
    {
        await UpdateRoleAndClaims(RoleDefaults.Admin, adminClaims);
        await UpdateRoleAndClaims(RoleDefaults.User, userClaims);
    }

    private async Task CreateDefaultSystemUserAsync()
    {
        await CreateUserAsync("system@defaul", "system", Configuration.SystemPassword, true, RoleDefaults.Admin);
    }

    private async Task UpdateRoleAndClaims(string roleName, IEnumerable<Claim> claims)
    {
        if (await roleManager.RoleExistsAsync(roleName) == false)
            await roleManager.CreateAsync(new Roles
            {
                Id = Guid.NewGuid(),
                Name = roleName,
                IsDefault = true
            });

        var role = await roleManager.FindByNameAsync(roleName);
        var currentClaims = await roleManager.GetClaimsAsync(role!);
        foreach (var claim in claims.Where(claim =>
                     !currentClaims.Any(element => element.Type == claim.Type && element.Value == claim.Value)))
            await roleManager.AddClaimAsync(role!, claim);
    }

    private async Task<Users> CreateUserAsync(string email, string username, string? password, bool isDefault,
        string roleName)
    {
        var user = await userManager.FindByNameAsync(username);
        if (user == null)
        {
            user = new Users
            {
                UserName = username,
                Email = email,
                FullName = username,
                Status = UserStatus.Active,
                EmailConfirmed = true,
                Avatar = null,
                IsDefault = isDefault,
                CreatedAt = DateTime.UtcNow
            };
            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                logger.LogInformation($"Create user {username} with password: {password}");
            }
            else
            {
                logger.LogError($"Create user {username} failed");
                foreach (var error in result.Errors) logger.LogError(error.Description);
            }
        }

        await userManager.AddToRoleAsync(user, roleName);
        return user;
    }

    private async Task CreateSampleData()
    {
        var max = 3;
        await CreateUserAsync("admin@local", "admin", "123qweA@", false, RoleDefaults.Admin);
        for (var i = 1; i < max; i++)
        {
            await CreateUserAsync($"user{i}@local", $"user{i}", "123qweA@", false, RoleDefaults.User);
            var repoUrl = $"https://gitlab.com/sample-project-{i}";
            var repoId = $"{i}";
            var repoName = $"Sample Project {i}";
            // sast scan
            var scanInfo = await ciService.InitScan(new CiScanRequest
            {
                Source = SourceType.GitLab,
                RepoId = repoId,
                RepoUrl = repoUrl,
                RepoName = repoName,
                GitAction = CommitType.CommitBranch,
                ScanTitle = $"Fix issue JIRA-23{i}",
                CommitBranch = "dev",
                CommitHash = $"35e32b6a00dec02ae7d7{i}",
                TargetBranch = null,
                MergeRequestId = null,
                Scanner = "semgrep",
                Type = ScannerType.Sast,
                JobUrl = "http://example.com",
                IsDefault = false,
            });
            logger.LogInformation("Init scan semgrep: " + scanInfo.ScanId);
            await ciService.UploadFinding(new CiUploadFindingRequest
            {
                ScanId = scanInfo.ScanId,
                Findings =
                [
                    new CiFinding
                    {
                        RuleId = "java.spring.security.tainted-ssrf-spring-add.tainted-ssrf-spring-add",
                        Identity = $"java.spring.security.tainted-ssrf-spring-add.tainted-ssrf-spring-add-{i}",
                        Name =
                            "Java Spring Security Tainted Ssrf Spring Add at src/main/java/com/scalesec/vulnado/LinkLister.java",
                        Description =
                            "Untrusted input might be used to build an HTTP request, which can lead to a Server-side request forgery (SSRF) vulnerability. SSRF allows an attacker to send crafted requests from the server side to other internal or external systems. SSRF can lead to unauthorized access to sensitive data and, in some cases, allow the attacker to control applications or systems that trust the vulnerable service. To prevent this vulnerability, avoid allowing user input to craft the base request. Instead, treat it as part of the path or query parameter and encode it appropriately. When user input is necessary to prepare the HTTP request, perform strict input validation. Additionally, whenever possible, use allowlists to only interact with expected, trusted domains.",
                        Category = null,
                        Recommendation = null,
                        Severity = FindingSeverity.Critical,
                        Location = new FindingLocation
                        {
                            Path = "src/main/java/com/scalesec/vulnado/LinkLister.java",
                            Snippet = "url",
                            StartLine = 10
                        },
                        Metadata = new FindingMetadata
                        {
                            FindingFlow =
                            [
                                new FindingLocation
                                {
                                    Path = "src/main/java/com/scalesec/vulnado/LinkController.java",
                                    Snippet = "input",
                                    StartLine = 6
                                },
                                new FindingLocation
                                {
                                    Path = "src/main/java/com/scalesec/vulnado/LinkLister.java",
                                    Snippet = "url",
                                    StartLine = 10
                                }
                            ],
                            References =
                            [
                                "https://semgrep.dev/r/java.spring.security.tainted-ssrf-spring-add.tainted-ssrf-spring-add"
                            ]
                        }
                    },
                    new CiFinding
                    {
                        RuleId =
                            "java.lang.security.audit.active-debug-code-printstacktrace.active-debug-code-printstacktrace",
                        Identity =
                            $"java.lang.security.audit.active-debug-code-printstacktrace.active-debug-code-printstacktrace-{i}",
                        Name =
                            "Java Lang Security Audit Active Debug Code Printstacktrace at src/main/java/com/scalesec/vulnado/Cowsay.java",
                        Description =
                            "Possible active debug code detected. Deploying an application with debug code can create unintended entry points or expose sensitive information.",
                        Category = null,
                        Recommendation = null,
                        Severity = FindingSeverity.Low,
                        Location = new FindingLocation
                        {
                            Path = "src/main/java/com/scalesec/vulnado/Cowsay.java",
                            Snippet = "e.printStacktrace()",
                            StartLine = 10
                        },
                        Metadata = new FindingMetadata
                        {
                            References =
                            [
                                "https://semgrep.dev/r?q=java.lang.security.audit.active-debug-code-printstacktrace.active-debug-code-printstacktrace"
                            ]
                        }
                    }
                ]
            });
            await ciService.UpdateScan(scanInfo.ScanId, new UpdateCiScanRequest
            {
                Status = ScanStatus.Completed
            });
            // dependency scan
            scanInfo = await ciService.InitScan(new CiScanRequest
            {
                Source = SourceType.GitLab,
                RepoId = repoId,
                RepoUrl = repoUrl,
                RepoName = repoName,
                GitAction = CommitType.CommitBranch,
                ScanTitle = $"Fix issue JIRA-23{i}",
                CommitBranch = "dev",
                CommitHash = $"35e32b6a00dec02ae7d7{i}",
                TargetBranch = null,
                MergeRequestId = null,
                Scanner = "trivy",
                Type = ScannerType.Dependency,
                JobUrl = "http://example.com",
                IsDefault = false,
            });
            logger.LogInformation("Init scan trivy: " + scanInfo.ScanId);
            await ciService.UploadDependency(new CiUploadDependencyRequest
            {
                ScanId = scanInfo.ScanId,
                Packages =
                [
                    new CiPackage
                    {
                        PkgId = "pkg:maven/org.springframework/spring-webmvc@4.1.6.RELEASE",
                        Group = "org.springframework",
                        Name = "spring-webmvc",
                        Version = "4.1.6.RELEASE",
                        Type = "pom",
                        Location = "pom.xml"
                    },
                    new CiPackage
                    {
                        PkgId = "pkg:maven/org.springframework/spring-core@4.1.6.RELEASE",
                        Group = "org.springframework",
                        Name = "spring-core",
                        Version = "4.1.6.RELEASE",
                        Type = "pom"
                    },
                    new CiPackage
                    {
                        PkgId = "pkg:maven/org.springframework/spring-beans@4.1.6.RELEASE",
                        Group = "org.springframework",
                        Name = "spring-beans",
                        Version = "4.1.6.RELEASE",
                        Type = "pom"
                    }
                ],
                PackageDependencies =
                [
                    new CiPackageDependency
                    {
                        PkgId = "pkg:maven/org.springframework/spring-webmvc@4.1.6.RELEASE",
                        Dependencies =
                        [
                            "pkg:maven/org.springframework/spring-core@4.1.6.RELEASE",
                            "pkg:maven/org.springframework/spring-beans@4.1.6.RELEASE"
                        ]
                    }
                ],
                Vulnerabilities =
                [
                    new CiVulnerability
                    {
                        Identity = "CVE-2022-22965",
                        Name = "CVE-2022-22965",
                        Description =
                            "A Spring MVC or Spring WebFlux application running on JDK 9+ may be vulnerable to remote code execution (RCE) via data binding. The specific exploit requires the application to run on Tomcat as a WAR deployment. If the application is deployed as a Spring Boot executable jar, i.e. the default, it is not vulnerable to the exploit. However, the nature of the vulnerability is more general, and there may be other ways to exploit it.",
                        FixedVersion = "4.1.19",
                        Severity = FindingSeverity.High,
                        PkgId = "pkg:maven/org.springframework/spring-webmvc@4.1.6.RELEASE",
                        PkgName = "org.springframework.spring-webmvc",
                        PublishedAt = null,
                        Metadata = new FindingMetadata
                        {
                            Cvss = "CVSS:3.1/AV:N/AC:L/PR:N/UI:N/S:U/C:H/I:H/A:H"
                        }
                    }
                ]
            });
            await ciService.UpdateScan(scanInfo.ScanId, new UpdateCiScanRequest
            {
                Status = ScanStatus.Completed
            });
        }
    }
}