using ControlledForms.IntegrationTests;
using SilveR.StatsModels;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace SilveR.IntegrationTests
{
    [Collection("Sequential")]
    public class SummaryStatisticsTests : IClassFixture<SilveRTestWebApplicationFactory<Startup>>
    {
        private readonly SilveRTestWebApplicationFactory<Startup> _factory;

        public SummaryStatisticsTests(SilveRTestWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }


        [Fact]
        public async Task SS1()
        {
            string testName = "SS1";

            // Arrange
            HttpClient client = _factory.CreateClient();

            SummaryStatisticsModel model = new SummaryStatisticsModel();
            model.DatasetID = _factory.SheetNames.Single(x => x.Value == "summary").Key;
            model.Responses = new string[] { "Resp1" };

            //Act
            HttpResponseMessage response = await client.PostAsync("Analyses/SummaryStatistics", new FormUrlEncodedContent(model.ToKeyValue()));
            IEnumerable<string> errors = await Helpers.ExtractErrors(response);

            //Assert
            Assert.Contains("The response variable selected (Resp1) contains non-numerical data. Please amend the dataset prior to running the analysis.", errors);
            Helpers.SaveOutput("SummaryStatistics", testName, errors);
        }

        [Fact]
        public async Task SS2()
        {
            string testName = "SS2";

            // Arrange
            HttpClient client = _factory.CreateClient();

            SummaryStatisticsModel model = new SummaryStatisticsModel();
            model.DatasetID = _factory.SheetNames.Single(x => x.Value == "summary").Key;
            model.Responses = new string[] { "Resp 2" };
            model.FirstCatFactor = "Cat1";

            //Act
            HttpResponseMessage response = await client.PostAsync("Analyses/SummaryStatistics", new FormUrlEncodedContent(model.ToKeyValue()));
            IEnumerable<string> errors = await Helpers.ExtractErrors(response);

            //Assert
            Assert.Contains("The categorisation factor selected (Cat1) contains missing data where there are observations present in the response variable. Please check the raw data and make sure the data was entered correctly.", errors);
            Helpers.SaveOutput("SummaryStatistics", testName, errors);
        }

        [Fact]
        public async Task SS3()
        {
            string testName = "SS3";

            // Arrange
            HttpClient client = _factory.CreateClient();

            SummaryStatisticsModel model = new SummaryStatisticsModel();
            model.DatasetID = _factory.SheetNames.Single(x => x.Value == "summary").Key;
            model.Responses = new string[] { "Resp 2" };
            model.FirstCatFactor = "Cat2";

            //Act
            HttpResponseMessage response = await client.PostAsync("Analyses/SummaryStatistics", new FormUrlEncodedContent(model.ToKeyValue()));
            IEnumerable<string> errors = await Helpers.ExtractErrors(response);

            //Assert
            Assert.Contains("There is no replication in one or more of the levels of the Response (Cat2). Please select another factor.", errors);
            Helpers.SaveOutput("SummaryStatistics", testName, errors);
        }

        [Fact]
        public async Task SS4()
        {
            string testName = "SS4";

            //Arrange
            HttpClient client = _factory.CreateClient();

            SummaryStatisticsModel model = new SummaryStatisticsModel();
            model.DatasetID = _factory.SheetNames.Single(x => x.Value == "summary").Key;
            model.Responses = new string[] { "Resp 2" };
            model.FirstCatFactor = "Cat3";

            //Act
            HttpResponseMessage response = await client.PostAsync("Analyses/SummaryStatistics", new FormUrlEncodedContent(model.ToKeyValue()));
            IEnumerable<string> errors = await Helpers.ExtractErrors(response);

            //Assert
            Assert.Contains("There are no observations recorded on the levels of the Response (Cat3). Please amend the dataset prior to running the analysis.", errors);
            Helpers.SaveOutput("SummaryStatistics", testName, errors);
        }

        [Fact]
        public async Task SS5()
        {
            string testName = "SS5";

            //Arrange
            HttpClient client = _factory.CreateClient();

            SummaryStatisticsModel model = new SummaryStatisticsModel();
            model.DatasetID = _factory.SheetNames.Single(x => x.Value == "summary").Key;
            model.Responses = new string[] { "Resp3" };
            model.Transformation = "Log10";

            //Act
            HttpResponseMessage response = await client.PostAsync("Analyses/SummaryStatistics", new FormUrlEncodedContent(model.ToKeyValue()));
            IEnumerable<string> warnings = await Helpers.ExtractWarnings(response);

            //Assert
            Assert.Contains("You have Log10 transformed the Resp3 variable. Unfortunately some of the Resp3 values are zero and/or negative. These values have been ignored in the analysis as it is not possible to transform them.", warnings);
            Helpers.SaveOutput("SummaryStatistics", testName, warnings);
        }

        [Fact]
        public async Task SS6()
        {
            string testName = "SS6";

            // Arrange
            HttpClient client = _factory.CreateClient();

            SummaryStatisticsModel model = new SummaryStatisticsModel();
            model.DatasetID = _factory.SheetNames.Single(x => x.Value == "summary").Key;
            model.Responses = new string[] { "Resp3" };
            model.Transformation = "Square Root";

            //Act
            HttpResponseMessage response = await client.PostAsync("Analyses/SummaryStatistics", new FormUrlEncodedContent(model.ToKeyValue()));
            IEnumerable<string> warnings = await Helpers.ExtractWarnings(response);

            //Assert
            Assert.Contains("You have Square Root transformed the Resp3 variable. Unfortunately some of the Resp3 values are negative. These values have been ignored in the analysis as it is not possible to transform them.", warnings);
            Helpers.SaveOutput("SummaryStatistics", testName, warnings);
        }

        [Fact]
        public async Task SS7()
        {
            string testName = "SS7";

            // Arrange
            HttpClient client = _factory.CreateClient();

            SummaryStatisticsModel model = new SummaryStatisticsModel();
            model.DatasetID = _factory.SheetNames.Single(x => x.Value == "summary").Key;
            model.Responses = new string[] { "Resp3" };
            model.Transformation = "Loge";

            //Act
            HttpResponseMessage response = await client.PostAsync("Analyses/SummaryStatistics", new FormUrlEncodedContent(model.ToKeyValue()));
            IEnumerable<string> warnings = await Helpers.ExtractWarnings(response);

            //Assert
            Assert.Contains("You have Loge transformed the Resp3 variable. Unfortunately some of the Resp3 values are zero and/or negative. These values have been ignored in the analysis as it is not possible to transform them.", warnings);
            Helpers.SaveOutput("SummaryStatistics", testName, warnings);
        }

        [Fact]
        public async Task SS8()
        {
            string testName = "SS8";

            // Arrange
            HttpClient client = _factory.CreateClient();

            SummaryStatisticsModel model = new SummaryStatisticsModel();
            model.DatasetID = _factory.SheetNames.Single(x => x.Value == "summary").Key;
            model.Responses = new string[] { "Resp3" };
            model.Transformation = "ArcSine";

            //Act
            HttpResponseMessage response = await client.PostAsync("Analyses/SummaryStatistics", new FormUrlEncodedContent(model.ToKeyValue()));
            IEnumerable<string> warnings = await Helpers.ExtractWarnings(response);

            //Assert
            Assert.Contains("You have ArcSine transformed the Resp3 variable. Unfortunately some of the Resp3 values are <0 or >1. These values have been ignored in the analysis as it is not possible to transform them.", warnings);
            Helpers.SaveOutput("SummaryStatistics", testName, warnings);
        }

        [Fact]
        public async Task SS9()
        {
            string testName = "SS9";

            // Arrange
            HttpClient client = _factory.CreateClient();

            SummaryStatisticsModel model = new SummaryStatisticsModel();
            model.DatasetID = _factory.SheetNames.Single(x => x.Value == "summary").Key;
            model.Responses = new string[] { "Resp11" };
            model.Transformation = "ArcSine";

            //Act
            HttpResponseMessage response = await client.PostAsync("Analyses/SummaryStatistics", new FormUrlEncodedContent(model.ToKeyValue()));
            IEnumerable<string> warnings = await Helpers.ExtractWarnings(response);

            //Assert
            Assert.Contains("You have ArcSine transformed the Resp11 variable. Unfortunately some of the Resp11 values are <0 or >1. These values have been ignored in the analysis as it is not possible to transform them.", warnings);
            Helpers.SaveOutput("SummaryStatistics", testName, warnings);
        }

        [Fact]
        public async Task SS10()
        {
            string testName = "SS10";

            // Arrange
            HttpClient client = _factory.CreateClient();

            SummaryStatisticsModel model = new SummaryStatisticsModel();
            model.DatasetID = _factory.SheetNames.Single(x => x.Value == "summary").Key;
            model.Responses = new string[] { "Resp1", "Resp 2" };

            //Act
            HttpResponseMessage response = await client.PostAsync("Analyses/SummaryStatistics", new FormUrlEncodedContent(model.ToKeyValue()));
            IEnumerable<string> errors = await Helpers.ExtractErrors(response);

            //Assert
            Assert.Contains("The response variable selected (Resp1) contains non-numerical data. Please amend the dataset prior to running the analysis.", errors);
            Helpers.SaveOutput("SummaryStatistics", testName, errors);
        }

        [Fact]
        public async Task SS11()
        {
            string testName = "SS11";

            // Arrange
            HttpClient client = _factory.CreateClient();

            SummaryStatisticsModel model = new SummaryStatisticsModel();
            model.DatasetID = _factory.SheetNames.Single(x => x.Value == "summary").Key;
            model.Responses = new string[] { "Resp 2", "Resp 5" };
            model.FirstCatFactor = "Cat1";
            model.SecondCatFactor = "Cat4";

            //Act
            HttpResponseMessage response = await client.PostAsync("Analyses/SummaryStatistics", new FormUrlEncodedContent(model.ToKeyValue()));
            IEnumerable<string> errors = await Helpers.ExtractErrors(response);

            //Assert
            Assert.Contains("The categorisation factor selected (Cat1) contains missing data where there are observations present in the response variable. Please check the raw data and make sure the data was entered correctly.", errors);
            Helpers.SaveOutput("SummaryStatistics", testName, errors);
        }

        [Fact]
        public async Task SS12()
        {
            string testName = "SS12";

            // Arrange
            HttpClient client = _factory.CreateClient();

            SummaryStatisticsModel model = new SummaryStatisticsModel();
            model.DatasetID = _factory.SheetNames.Single(x => x.Value == "summary").Key;
            model.Responses = new string[] { "Resp 2" };
            model.FirstCatFactor = "Cat2";
            model.SecondCatFactor = "Cat4";

            //Act
            HttpResponseMessage response = await client.PostAsync("Analyses/SummaryStatistics", new FormUrlEncodedContent(model.ToKeyValue()));
            IEnumerable<string> errors = await Helpers.ExtractErrors(response);

            //Assert
            Assert.Contains("There is no replication in one or more of the levels of the Response (Cat2). Please select another factor.", errors);
            Helpers.SaveOutput("SummaryStatistics", testName, errors);
        }

        [Fact]
        public async Task SS13()
        {
            string testName = "SS13";

            // Arrange
            HttpClient client = _factory.CreateClient();

            SummaryStatisticsModel model = new SummaryStatisticsModel();
            model.DatasetID = _factory.SheetNames.Single(x => x.Value == "summary").Key;
            model.Responses = new string[] { "Resp 2" };
            model.FirstCatFactor = "Cat3";
            model.SecondCatFactor = "Cat4";

            //Act
            HttpResponseMessage response = await client.PostAsync("Analyses/SummaryStatistics", new FormUrlEncodedContent(model.ToKeyValue()));
            IEnumerable<string> errors = await Helpers.ExtractErrors(response);

            //Assert
            Assert.Contains("There are no observations recorded on the levels of the Response (Cat3). Please amend the dataset prior to running the analysis.", errors);
            Helpers.SaveOutput("SummaryStatistics", testName, errors);
        }

        [Fact]
        public async Task SS14()
        {
            string testName = "SS14";

            // Arrange
            HttpClient client = _factory.CreateClient();

            SummaryStatisticsModel model = new SummaryStatisticsModel();
            model.DatasetID = _factory.SheetNames.Single(x => x.Value == "summary").Key;
            model.Responses = new string[] { "Resp 2" };
            model.Significance = 0.95m;

            //Act
            HttpResponseMessage response = await client.PostAsync("Analyses/SummaryStatistics", new FormUrlEncodedContent(model.ToKeyValue()));
            IEnumerable<string> errors = await Helpers.ExtractErrors(response);

            //Assert
            Assert.Contains("You have selected a confidence limit that is less than 1. Note that this value should be entered as a percentage and not a fraction.", errors);
            Helpers.SaveOutput("SummaryStatistics", testName, errors);
        }

        [Fact]
        public async Task SS15()
        {
            string testName = "SS15";

            // Arrange
            HttpClient client = _factory.CreateClient();

            SummaryStatisticsModel model = new SummaryStatisticsModel();
            model.DatasetID = _factory.SheetNames.Single(x => x.Value == "summary").Key;
            model.Responses = new string[] { "Resp3", "Resp 2" };
            model.Transformation = "Log10";

            //Act
            HttpResponseMessage response = await client.PostAsync("Analyses/SummaryStatistics", new FormUrlEncodedContent(model.ToKeyValue()));
            IEnumerable<string> warnings = await Helpers.ExtractWarnings(response);

            //Assert
            Assert.Contains("You have Log10 transformed the Resp3 variable. Unfortunately some of the Resp3 values are zero and/or negative. These values have been ignored in the analysis as it is not possible to transform them.", warnings);
            Helpers.SaveOutput("SummaryStatistics", testName, warnings);
        }

        [Fact]
        public async Task SS16()
        {
            string testName = "SS16";

            // Arrange
            HttpClient client = _factory.CreateClient();

            SummaryStatisticsModel model = new SummaryStatisticsModel();
            model.DatasetID = _factory.SheetNames.Single(x => x.Value == "summary").Key;
            model.Responses = new string[] { "Resp3", "Resp 2" };
            model.Transformation = "Square Root";

            //Act
            HttpResponseMessage response = await client.PostAsync("Analyses/SummaryStatistics", new FormUrlEncodedContent(model.ToKeyValue()));
            IEnumerable<string> warnings = await Helpers.ExtractWarnings(response);

            //Assert
            Assert.Contains("You have Square Root transformed the Resp3 variable. Unfortunately some of the Resp3 values are negative. These values have been ignored in the analysis as it is not possible to transform them.", warnings);
            Helpers.SaveOutput("SummaryStatistics", testName, warnings);
        }

        [Fact]
        public async Task SS17()
        {
            string testName = "SS17";

            // Arrange
            HttpClient client = _factory.CreateClient();

            SummaryStatisticsModel model = new SummaryStatisticsModel();
            model.DatasetID = _factory.SheetNames.Single(x => x.Value == "summary").Key;
            model.Responses = new string[] { "Resp4", "Resp 2" };
            model.Transformation = "Loge";

            //Act
            HttpResponseMessage response = await client.PostAsync("Analyses/SummaryStatistics", new FormUrlEncodedContent(model.ToKeyValue()));
            IEnumerable<string> warnings = await Helpers.ExtractWarnings(response);

            //Assert
            Assert.Contains("You have Loge transformed the Resp4 variable. Unfortunately some of the Resp4 values are zero and/or negative. These values have been ignored in the analysis as it is not possible to transform them.", warnings);
            Helpers.SaveOutput("SummaryStatistics", testName, warnings);
        }

        [Fact]
        public async Task SS18()
        {
            string testName = "SS18";

            // Arrange
            HttpClient client = _factory.CreateClient();

            SummaryStatisticsModel model = new SummaryStatisticsModel();
            model.DatasetID = _factory.SheetNames.Single(x => x.Value == "summary").Key;
            model.Responses = new string[] { "Resp6", "Resp7" };
            model.FirstCatFactor = "Cat4";

            //Act
            HttpResponseMessage response = await client.PostAsync("Analyses/SummaryStatistics", new FormUrlEncodedContent(model.ToKeyValue()));
            IEnumerable<string> errors = await Helpers.ExtractErrors(response);

            //Assert
            Assert.Contains("There are no observations recorded on the levels of the Response (Cat4). Please amend the dataset prior to running the analysis.", errors);
            Helpers.SaveOutput("SummaryStatistics", testName, errors);
        }

        [Fact]
        public async Task SS19()
        {
            string testName = "SS19";

            // Arrange
            HttpClient client = _factory.CreateClient();

            SummaryStatisticsModel model = new SummaryStatisticsModel();
            model.DatasetID = _factory.SheetNames.Single(x => x.Value == "summary").Key;
            model.Responses = new string[] { "Resp1" };
            model.Mean = false;
            model.N = false;
            model.StandardDeviation = false;
            model.Variance = false;
            model.StandardErrorOfMean = false;
            model.MinAndMax = false;
            model.MedianAndQuartiles = false;
            model.CoefficientOfVariation = false;
            model.NormalProbabilityPlot = false;
            model.CoefficientOfVariation = false;
            model.ByCategoriesAndOverall = false;

            //Act
            HttpResponseMessage response = await client.PostAsync("Analyses/SummaryStatistics", new FormUrlEncodedContent(model.ToKeyValue()));
            IEnumerable<string> errors = await Helpers.ExtractErrors(response);

            //Assert
            Assert.Contains("You have not selected anything to output!", errors);
            Helpers.SaveOutput("SummaryStatistics", testName, errors);
        }

        //[Fact]
        //public async Task SS20()
        //{
        //    // Arrange
        //    HttpClient client = _factory.CreateClient();

        //    SummaryStatisticsModel model = new SummaryStatisticsModel();
        //    model.DatasetID = _factory.SheetNames.Single(x => x.Value == "summary").Key;
        //    model.Responses = new string[] { "Resp6", "Resp7" };
        //    model.FirstCatFactor = "Cat4";

        //    //Act
        //    HttpResponseMessage response = await client.PostAsync("Analyses/SummaryStatistics", new FormUrlEncodedContent(model.ToKeyValue()));
        //    IEnumerable<string> errors = await Helpers.ExtractErrors(response);

        //    //Assert
        //    Assert.Contains("There are no observations recorded on the levels of the Response (Cat4). Please amend the dataset prior to running the analysis.", errors);
        //}

        //SS21 - Not required

        [Fact]
        public async Task SS22()
        {
            string testName = "SS22";

            // Arrange
            HttpClient client = _factory.CreateClient();

            SummaryStatisticsModel model = new SummaryStatisticsModel();
            model.DatasetID = _factory.SheetNames.Single(x => x.Value == "summary").Key;
            model.Responses = new string[] { "Resp 2" };
            model.Transformation = "Loge";
            model.Mean = true;
            model.N = true;
            model.StandardDeviation = true;
            model.Variance = true;
            model.StandardErrorOfMean = true;
            model.MinAndMax = true;
            model.MedianAndQuartiles = true;
            model.CoefficientOfVariation = true;
            model.NormalProbabilityPlot = true;
            model.CoefficientOfVariation = true;
            model.ByCategoriesAndOverall = true;

            //Act
            string htmlResults = await Helpers.SubmitAnalysis(client, "SummaryStatistics", new FormUrlEncodedContent(model.ToKeyValue()));
            Helpers.SaveHtmlOutput("SummaryStatistics", testName, htmlResults);

            //Assert
            string expectedHtml = File.ReadAllText(Path.Combine("ExpectedResults", "SummaryStatistics", testName+".html"));
            Assert.Equal(expectedHtml, htmlResults);
        }

        [Fact]
        public async Task SS23()
        {
            string testName = "SS23";

            // Arrange
            HttpClient client = _factory.CreateClient();

            SummaryStatisticsModel model = new SummaryStatisticsModel();
            model.DatasetID = _factory.SheetNames.Single(x => x.Value == "summary").Key;
            model.Responses = new string[] { "Resp 2", "Resp8", "Resp9" };
            model.Transformation = "Log10";
            model.Mean = true;
            model.N = true;
            model.StandardDeviation = true;
            model.Variance = true;
            model.StandardErrorOfMean = true;
            model.MinAndMax = true;
            model.MedianAndQuartiles = true;
            model.CoefficientOfVariation = true;
            model.NormalProbabilityPlot = true;
            model.CoefficientOfVariation = true;
            model.ByCategoriesAndOverall = true;

            //Act
            string htmlResults = await Helpers.SubmitAnalysis(client, "SummaryStatistics", new FormUrlEncodedContent(model.ToKeyValue()));
            Helpers.SaveHtmlOutput("SummaryStatistics", testName, htmlResults);

            //Assert
            string expectedHtml = File.ReadAllText(Path.Combine("ExpectedResults", "SummaryStatistics", testName + ".html"));
            Assert.Equal(expectedHtml, htmlResults);
        }

        [Fact]
        public async Task SS24()
        {
            string testName = "SS24";

            // Arrange
            HttpClient client = _factory.CreateClient();

            SummaryStatisticsModel model = new SummaryStatisticsModel();
            model.DatasetID = _factory.SheetNames.Single(x => x.Value == "summary").Key;
            model.Responses = new string[] { "Resp 2" };
            model.FirstCatFactor = "Cat4";
            model.Transformation = "Square Root";
            model.Mean = true;
            model.N = true;
            model.StandardDeviation = true;
            model.Variance = true;
            model.StandardErrorOfMean = true;
            model.MinAndMax = true;
            model.MedianAndQuartiles = true;
            model.CoefficientOfVariation = true;
            model.NormalProbabilityPlot = true;
            model.CoefficientOfVariation = true;
            model.ByCategoriesAndOverall = true;
            model.Significance = 90;

            //Act
            string htmlResults = await Helpers.SubmitAnalysis(client, "SummaryStatistics", new FormUrlEncodedContent(model.ToKeyValue()));
            Helpers.SaveHtmlOutput("SummaryStatistics", testName, htmlResults);

            //Assert
            string expectedHtml = File.ReadAllText(Path.Combine("ExpectedResults", "SummaryStatistics", testName + ".html"));
            Assert.Equal(expectedHtml, htmlResults);
        }

        [Fact]
        public async Task SS25()
        {
            string testName = "SS25";

            // Arrange
            HttpClient client = _factory.CreateClient();

            SummaryStatisticsModel model = new SummaryStatisticsModel();
            model.DatasetID = _factory.SheetNames.Single(x => x.Value == "summary").Key;
            model.Responses = new string[] { "Resp 2", "Resp8", "Resp9" };
            model.FirstCatFactor = "Cat4";
            model.Transformation = "Square Root";
            model.Mean = true;
            model.N = true;
            model.StandardDeviation = true;
            model.Variance = true;
            model.StandardErrorOfMean = true;
            model.MinAndMax = true;
            model.MedianAndQuartiles = true;
            model.CoefficientOfVariation = true;
            model.NormalProbabilityPlot = true;
            model.CoefficientOfVariation = true;
            model.ByCategoriesAndOverall = true;
            model.Significance = 99;

            //Act
            string htmlResults = await Helpers.SubmitAnalysis(client, "SummaryStatistics", new FormUrlEncodedContent(model.ToKeyValue()));
            Helpers.SaveHtmlOutput("SummaryStatistics", testName, htmlResults);

            //Assert
            string expectedHtml = File.ReadAllText(Path.Combine("ExpectedResults", "SummaryStatistics", testName + ".html"));
            Assert.Equal(expectedHtml, htmlResults);
        }

        [Fact]
        public async Task SS26()
        {
            string testName = "SS26";

            // Arrange
            HttpClient client = _factory.CreateClient();

            SummaryStatisticsModel model = new SummaryStatisticsModel();
            model.DatasetID = _factory.SheetNames.Single(x => x.Value == "summary").Key;
            model.Responses = new string[] { "Resp 2" };
            model.FirstCatFactor = "Cat4";
            model.SecondCatFactor = "Cat5";
            model.ThirdCatFactor = "Cat6";
            model.Transformation = "Square Root";
            model.Mean = true;
            model.N = true;
            model.StandardDeviation = true;
            model.Variance = true;
            model.StandardErrorOfMean = true;
            model.MinAndMax = true;
            model.MedianAndQuartiles = true;
            model.CoefficientOfVariation = true;
            model.NormalProbabilityPlot = true;
            model.CoefficientOfVariation = true;
            model.ByCategoriesAndOverall = true;
            model.Significance = 95;

            //Act
            string htmlResults = await Helpers.SubmitAnalysis(client, "SummaryStatistics", new FormUrlEncodedContent(model.ToKeyValue()));
            Helpers.SaveHtmlOutput("SummaryStatistics", testName, htmlResults);

            //Assert
            string expectedHtml = File.ReadAllText(Path.Combine("ExpectedResults", "SummaryStatistics", testName + ".html"));
            Assert.Equal(expectedHtml, htmlResults);
        }

        [Fact]
        public async Task SS27()
        {
            string testName = "SS27";

            // Arrange
            HttpClient client = _factory.CreateClient();

            SummaryStatisticsModel model = new SummaryStatisticsModel();
            model.DatasetID = _factory.SheetNames.Single(x => x.Value == "summary").Key;
            model.Responses = new string[] { "Resp 2", "Resp8", "Resp9" };
            model.FirstCatFactor = "Cat4";
            model.SecondCatFactor = "Cat5";
            model.ThirdCatFactor = "Cat6";
            model.Transformation = "Log10";
            model.Mean = true;
            model.N = true;
            model.StandardDeviation = true;
            model.Variance = true;
            model.StandardErrorOfMean = true;
            model.MinAndMax = true;
            model.MedianAndQuartiles = true;
            model.CoefficientOfVariation = true;
            model.NormalProbabilityPlot = true;
            model.CoefficientOfVariation = true;
            model.ByCategoriesAndOverall = true;
            model.Significance = 95;

            //Act
            string htmlResults = await Helpers.SubmitAnalysis(client, "SummaryStatistics", new FormUrlEncodedContent(model.ToKeyValue()));
            Helpers.SaveHtmlOutput("SummaryStatistics", testName, htmlResults);

            //Assert
            string expectedHtml = File.ReadAllText(Path.Combine("ExpectedResults", "SummaryStatistics", testName + ".html"));
            Assert.Equal(expectedHtml, htmlResults);
        }

        [Fact]
        public async Task SS28()
        {
            string testName = "SS28";

            // Arrange
            HttpClient client = _factory.CreateClient();

            SummaryStatisticsModel model = new SummaryStatisticsModel();
            model.DatasetID = _factory.SheetNames.Single(x => x.Value == "summary").Key;
            model.Responses = new string[] { "Resp 2", "Resp8", "Resp9" };
            model.FirstCatFactor = "Cat6";
            model.Transformation = "None";
            model.Mean = true;
            model.N = true;
            model.StandardDeviation = true;
            model.Variance = true;
            model.StandardErrorOfMean = true;
            model.MinAndMax = true;
            model.MedianAndQuartiles = true;
            model.CoefficientOfVariation = true;
            model.NormalProbabilityPlot = true;
            model.CoefficientOfVariation = true;
            model.ByCategoriesAndOverall = true;
            model.Significance = 95;

            //Act
            string htmlResults = await Helpers.SubmitAnalysis(client, "SummaryStatistics", new FormUrlEncodedContent(model.ToKeyValue()));
            Helpers.SaveHtmlOutput("SummaryStatistics", testName, htmlResults);

            //Assert
            string expectedHtml = File.ReadAllText(Path.Combine("ExpectedResults", "SummaryStatistics", testName + ".html"));
            Assert.Equal(expectedHtml, htmlResults);
        }

        [Fact]
        public async Task SS29()
        {
            string testName = "SS29";

            // Arrange
            HttpClient client = _factory.CreateClient();

            SummaryStatisticsModel model = new SummaryStatisticsModel();
            model.DatasetID = _factory.SheetNames.Single(x => x.Value == "summary").Key;
            model.Responses = new string[] { "Resp 2", "Resp3", "Resp6" };
            model.FirstCatFactor = "Cat6";
            model.Transformation = "ArcSine";
            model.Mean = true;
            model.N = true;
            model.StandardDeviation = true;
            model.Variance = true;
            model.StandardErrorOfMean = true;
            model.MinAndMax = true;
            model.MedianAndQuartiles = true;
            model.CoefficientOfVariation = true;
            model.NormalProbabilityPlot = true;
            model.CoefficientOfVariation = true;
            model.ByCategoriesAndOverall = true;
            model.Significance = 95;

            //Act1
            HttpResponseMessage response = await client.PostAsync("Analyses/SummaryStatistics", new FormUrlEncodedContent(model.ToKeyValue()));
            IEnumerable<string> warnings = await Helpers.ExtractWarnings(response);

            //Assert
            Assert.Contains("You have ArcSine transformed the Resp3 variable. Unfortunately some of the Resp3 values are <0 or >1. These values have been ignored in the analysis as it is not possible to transform them.", warnings);
            Helpers.SaveOutput("SummaryStatistics", testName, warnings);

            //Act2 - ignore warnings
            var modelIgnoreWarnings = model.ToKeyValue();
            modelIgnoreWarnings.Add("ignoreWarnings", "true");
            string htmlResults = await Helpers.SubmitAnalysis(client, "SummaryStatistics", new FormUrlEncodedContent(modelIgnoreWarnings));
            Helpers.SaveHtmlOutput("SummaryStatistics", testName, htmlResults);

            //Assert
            string expectedHtml = File.ReadAllText(Path.Combine("ExpectedResults", "SummaryStatistics", testName + ".html"));
            Assert.Equal(expectedHtml, htmlResults);
        }
    }
}