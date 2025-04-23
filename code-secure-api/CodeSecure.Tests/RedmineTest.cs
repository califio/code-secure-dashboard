using System.Collections.Specialized;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Wordprocessing;
using Redmine.Net.Api;
using Redmine.Net.Api.Net;
using Redmine.Net.Api.Types;

namespace CodeSecure.Tests;

public class RedmineTest
{
    public string RedmineToken { get; set; } = string.Empty;
    public string RedmineUrl { get; set; } = string.Empty;
    public required RedmineManager Rm {get; set;}
    [SetUp]
    public void Setup()
    {
        RedmineToken = "18c047bfdde1c256e6c3ab2f4f1276faa00a4377";
        RedmineUrl = "http://localhost:8300";
        Rm = new RedmineManager(new RedmineManagerOptionsBuilder()
            .WithHost(RedmineUrl)
            .WithApiKeyAuthentication(RedmineToken));
    }

    [Test]
    public void GetProjectsTest()
    {
        var projects = Rm.Get<Project>();
        foreach (var project in projects)
        {
            Console.WriteLine(project.Id);
            Console.WriteLine(project.Name);
        }
    }
    [Test]
    public void FindingIssueBySubjectTest()
    {
        var parameters = new NameValueCollection
        {
            { "subject", "[iLead] Upgrade package spring.boot@5.1.1 to version 5.1.2 at pom.xml" }, // "~" nghĩa là chứa từ "bug"
            { "limit", "1" },
            { "project_id", "1" }
        };
        var projects = Rm.Get<Issue>(new RequestOptions{ QueryString = parameters});
        foreach (var project in projects)
        {
            Console.WriteLine(project.Id);
            Console.WriteLine(project.Subject);
        }
    }
    
    [Test]
    public void GetProjectByIdTest()
    {
        var project = Rm.Get<Project>("2");
        Console.WriteLine(project.Id);
        Console.WriteLine(project.Name);
        Console.WriteLine(project.Identifier);
    }

    [Test]
    public void CreateIssueTest()
    {
        var project = Rm.Get<Project>("attt");
        
        Console.WriteLine(project.Name);
        Console.WriteLine(project.Id);
        Console.WriteLine(project.Identifier);
        var issue = new Issue
        {
            Subject = "Bug Report 1",
            Description = "*Bug Report*\n 1. This is a bug report.\n2. This is a bug report.\n3. This is a bug report.",
            DueDate = DateTime.Now.AddDays(4),
            Project = project,
            Status = IdentifiableName.Create<IdentifiableName>(1),
            Tracker = IdentifiableName.Create<IdentifiableName>(1),
            Priority = IdentifiableName.Create<IdentifiableName>(1),
        };
        var ticket = Rm.Create(issue);
        Console.WriteLine(ticket.Description);
    }
}