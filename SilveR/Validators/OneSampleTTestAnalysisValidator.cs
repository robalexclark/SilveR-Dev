﻿using SilveR.StatsModels;

namespace SilveR.Validators
{
    public class OneSampleTTestAnalysisValidator : ValidatorBase
    {
        private readonly OneSampleTTestAnalysisModel osttVariables;

        public OneSampleTTestAnalysisValidator(OneSampleTTestAnalysisModel ostt)
            : base(ostt.DataTable)
        {
            osttVariables = ostt;
        }

        public override ValidationInfo Validate()
        {
            //go through all the column names, if any are numeric then stop the analysis
            if (!CheckColumnNames(osttVariables.Responses))
                return ValidationInfo;

            //Go through each response
            foreach (string response in osttVariables.Responses)
            {
                if (!CheckIsNumeric(response))
                {
                    ValidationInfo.AddErrorMessage("The Response (" + response + ") contains non-numeric data that cannot be processed. Please check the data and make sure it was entered correctly.");
                    return ValidationInfo;
                }

                if (CountResponses(response) <= 1)
                {
                    ValidationInfo.AddErrorMessage("There is no replication in the response variable (" + response + "). Please select another factor.");
                    return ValidationInfo;
                }
            }

            CheckTransformations(osttVariables.ResponseTransformation, osttVariables.Responses);

            //if get here then no errors so return true
            return ValidationInfo;
        }
    }
}