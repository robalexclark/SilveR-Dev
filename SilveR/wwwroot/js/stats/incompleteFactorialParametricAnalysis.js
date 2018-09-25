$(function () {
    jQuery.ajaxSettings.traditional = true

    $("#Response").kendoDropDownList({
        dataSource: theModel.availableVariablesAllowNull
    });

    $("#ResponseTransformation").kendoDropDownList({
        dataSource: theModel.transformationsList,
        change: function () {
            var covariateTransformation = $("#CovariateTransformation").data("kendoDropDownList");
            covariateTransformation.value($("#ResponseTransformation").val());
        }
    });

    $("#Treatments").kendoMultiSelect({
        dataSource: theModel.availableVariables,
        value: theModel.treatments,
        change: treatmentsChanged
    });

    $("#OtherDesignFactors").kendoMultiSelect({
        dataSource: theModel.availableVariables,
        value: theModel.otherDesignFactors
    });

    $("#Covariates").kendoMultiSelect({
        dataSource: theModel.availableVariables,
        value: theModel.covariates,
        change: covariateBlockEnableDisable
    });

    var primaryFactorDropdown = $("#PrimaryFactor").kendoDropDownList({
        dataSource: {
            transport: {
                read: {
                    url: "/Values/GetSelectedTreatments"
                }
            }
        }

    }).data("kendoDropDownList");
    primaryFactorDropdown.bind("dataBound", function (e) {
        primaryFactorDropdown.value(theModel.primaryFactor);
    });

    $("#CovariateTransformation").kendoDropDownList({
        dataSource: theModel.transformationsList
    });

    $("#Significance").kendoDropDownList({
        dataSource: theModel.significancesList
    });

    var selectedEffect = $("#SelectedEffect").kendoDropDownList({
        dataSource: {
            transport: {
                read: {
                    url: "/Values/GetSelectedEffectsList"
                }
            }
        },
        change: selectedEffectChanged
    }).data("kendoDropDownList");
    selectedEffect.bind("dataBound", function (e) {
        if (selectedEffect.select() === -1) {
            if (theModel.selectedEffect) {
                selectedEffect.value(theModel.selectedEffect);
            }
            else {
                selectedEffect.select(0);
            }
        }

        selectedEffect.enable(true);

        selectedEffectChanged();
    });

    $("#AllPairwise").kendoDropDownList({
        dataSource: theModel.pairwiseTestList
    });

    $("#ComparisonsBackToControl").kendoDropDownList({
        dataSource: theModel.comparisonsBackToControlTestList
    });

    var controlGroup = $("#ControlGroup").kendoDropDownList({
        autoBind: false,
        dataSource: {
            transport: {
                read: {
                    url: "/Values/GetLevels"
                }
            }
        }
    }).data("kendoDropDownList");
    controlGroup.bind("dataBound", function (e) {
        controlGroup.value(theModel.controlGroup);
    });

    treatmentsChanged();

    covariateBlockEnableDisable();
});

function treatmentsChanged() {
    var treatmentMultiSelect = $("#Treatments").data("kendoMultiSelect");

    $.ajax({
        type: 'GET',
        url: "/Values/GetInteractions",
        data: { selectedTreatments: treatmentMultiSelect.dataItems() },
        success: function (data) {
            var markup = '';
            for (var x = 0; x < data.length; x++) {
                markup += '<option value="' + data[x] + '">' + data[x] + '</option>';
            }

            $('#Interactions').html(markup).show();
        }
    });


    var currentlySelectedTreatments = $("#Treatments").data("kendoMultiSelect").dataItems();

    //treatments have changed so fill in the primary factor...
    var primaryFactorDropDown = $("#PrimaryFactor").data("kendoDropDownList");
    primaryFactorDropDown.dataSource.read({
        selectedTreatments: currentlySelectedTreatments
    });

    //...and the selected effect
    var selectedEffectDropDown = $("#SelectedEffect").data("kendoDropDownList");
    selectedEffectDropDown.dataSource.read({
        selectedTreatments: currentlySelectedTreatments
    });

    //selectedEffectsBlockEnableDisable();
}

//if the selected effect is changed then fill in the control group
function selectedEffectChanged() {

    var controlGroup = $("#ControlGroup").data("kendoDropDownList");

    controlGroup.value(null);
    controlGroup.dataSource.read({ treatment: $("#SelectedEffect").val(), datasetID: $("#DatasetID").val(), includeNull: true });

    selectedEffectsBlockEnableDisable();
}

function covariateBlockEnableDisable() {
    var covariateDropDown = $("#Covariates");
    var primaryFactorDropDown = $("#PrimaryFactor").data("kendoDropDownList");
    var covariateTransformationDropDown = $("#CovariateTransformation").data("kendoDropDownList");

    if (covariateDropDown.val().length > 0) {
        primaryFactorDropDown.enable(true);
        covariateTransformationDropDown.enable(true);
    }
    else {
        primaryFactorDropDown.enable(false);
        covariateTransformationDropDown.enable(false);
        covariateTransformationDropDown.value("None");
        primaryFactorDropDown.value(null);
    }
}

function selectedEffectsBlockEnableDisable() {
    var treatmentMultiSelect = $("#Treatments").data("kendoMultiSelect");
    var selectedEffectDropDown = $("#SelectedEffect").data("kendoDropDownList");
    var lsMeansSelectedCheckBox = $("#LSMeansSelected");
    var allPairwise = $('#AllPairwise').data("kendoDropDownList");
    var comparisonsBackToControl = $('#ComparisonsBackToControl').data("kendoDropDownList");
    var controlGroup = $('#ControlGroup').data("kendoDropDownList");

    if (treatmentMultiSelect != null && treatmentMultiSelect.value().length > 0 && selectedEffectDropDown.value().indexOf("*") == -1) {
        //selectedEffectDropDown.enable(true);
        lsMeansSelectedCheckBox.prop("disabled", false);
        allPairwise.enable(true);
        comparisonsBackToControl.enable(true);
        controlGroup.enable(true);
    }
    else {
        //selectedEffectDropDown.enable(false);
        lsMeansSelectedCheckBox.prop("disabled", true);
        allPairwise.enable(false);
        comparisonsBackToControl.enable(false);
        controlGroup.enable(false);
        allPairwise.value(null);
        comparisonsBackToControl.value(null);
        controlGroup.value(null);
    }

    //if (selectedEffectDropDown.value().indexOf("*") >= 0) {
    //    comparisonsBackToControl.enable(false);
    //    controlGroup.enable(false);
    //    comparisonsBackToControl.value(null);
    //    controlGroup.value(null);
    //}
}