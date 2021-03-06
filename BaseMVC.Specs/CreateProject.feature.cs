// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.6.1.0
//      SpecFlow Generator Version:1.6.0.0
//      Runtime Version:4.0.30319.235
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
namespace BaseMVC.Specs
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.6.1.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("New project creation")]
    public partial class NewProjectCreationFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "CreateProject.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "New project creation", "  In order to manage projects\r\n  As a project manager\r\n  I want to be able to cre" +
                    "ate new project", GenerationTargetLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Open New Project page as Project Manager")]
        public virtual void OpenNewProjectPageAsProjectManager()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Open New Project page as Project Manager", ((string[])(null)));
#line 6
this.ScenarioSetup(scenarioInfo);
#line 7
    testRunner.Given("I am Project Manager");
#line 8
    testRunner.When("I open New Project page");
#line 9
    testRunner.Then("New Project page will be filled with default values");
#line hidden
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("After saving New Project we are redirected to Project List page")]
        public virtual void AfterSavingNewProjectWeAreRedirectedToProjectListPage()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("After saving New Project we are redirected to Project List page", ((string[])(null)));
#line 11
this.ScenarioSetup(scenarioInfo);
#line 12
    testRunner.Given("I opened New Project page");
#line 13
    testRunner.And("I am Project Manager");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table1.AddRow(new string[] {
                        "Name",
                        "Sample project"});
            table1.AddRow(new string[] {
                        "StartDate",
                        "2011-07-01"});
#line 14
    testRunner.And("I filled New Project page as follows", ((string)(null)), table1);
#line 18
    testRunner.When("I press Save button");
#line 19
    testRunner.Then("I will be redirected to Project List page");
#line hidden
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Saving New Project stores it in database")]
        public virtual void SavingNewProjectStoresItInDatabase()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Saving New Project stores it in database", ((string[])(null)));
#line 21
this.ScenarioSetup(scenarioInfo);
#line 22
    testRunner.Given("I opened New Project page");
#line 23
    testRunner.And("I am Project Manager");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table2.AddRow(new string[] {
                        "Name",
                        "Sample project"});
            table2.AddRow(new string[] {
                        "StartDate",
                        "2011-07-01"});
#line 24
    testRunner.And("I filled New Project page as follows", ((string)(null)), table2);
#line 28
    testRunner.When("I press Save button");
#line 29
    testRunner.Then("project will be saved in database");
#line hidden
            testRunner.CollectScenarioErrors();
        }
    }
}
#endregion
