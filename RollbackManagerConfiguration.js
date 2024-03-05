// JavaScript source code

// Entity
const Configuration = {
    LogicalName: "cr70c_rollbackmanagerlabconfiguration",
    Fields: {
        TransactionType: "cr70c_transactiontype",
        TransactionTypeValues: "cr70c_transactiontypevalues",
        EntityLogicalName: "cr70c_entitylogicalname",
        ApiKeyId: "cr70c_rollbackmanagerlabapikey",
    },
    getAttributeValue: (attr) => Xrm.Page.getAttribute(attr).getValue(),
    setAttributeValue: (attr, val) => Xrm.Page.getAttribute(attr).setValue(val),
    getFormType: () => Xrm.Page.ui.getFormType(),
    retrieveMultipleAsync: async (query) => await Xrm.WebApi.retrieveMultipleRecords(Configuration.LogicalName, query),
    setDisabled: (attr, isDisabled) => Xrm.Page.getControl(attr).setDisabled(isDisabled),
    showError: (errorCode, details, message) => Xrm.Navigation.openErrorDialog({ errorCode: errorCode, details: details, message: message })
};

// Form Type Values
const FormTypes = {
    Create: 1,
    Update: 2,
}

function formOnLoad(executionContext) {
    // Form Initial Actions
    disableFieldsIfFormIsSaved();
}

function transactionTypeOnChange() {
    // Update TransactionTypeValues
    const transactionTypes = Configuration.getAttributeValue(Configuration.Fields.TransactionType);

    if (transactionTypes && transactionTypes.length > 0) {
        const transactionTypeValues = transactionTypes.join(';');
        Configuration.setAttributeValue(Configuration.Fields.TransactionTypeValues, transactionTypeValues);
    } else {
        Configuration.setAttributeValue(Configuration.Fields.TransactionTypeValues, null);
    }
}

function disableFieldsIfFormIsSaved() {
    // Your logic for disabling fields if the form is saved
    if (Configuration.getFormType() != FormTypes.Create) {
        Configuration.setDisabled(Configuration.Fields.EntityLogicalName, true);
        Configuration.setDisabled(Configuration.Fields.ApiKeyId, true);
    }
}

function entityLogicalNameOnChange() {
    // Clean WhiteSpaces
    const entityLogicalName = Configuration.getAttributeValue(Configuration.Fields.EntityLogicalName);

    if (entityLogicalName) {

        const trimmedValue = entityLogicalName.trim();

        Xrm.Utility.getEntityMetadata(trimmedValue, []).then(function (entityMetadata) {

            if (entityMetadata != undefined) {

                checkHasMultipleConfiguration().then(hasMultipleConfiguration => {
                    if (hasMultipleConfiguration) {
                        // Prevent Save
                        Configuration.setAttributeValue(Configuration.Fields.EntityLogicalName, null);

                        //Show Error
                        Configuration.showError("RLBM-EX-0001", "You Have Already Set Up Configuration For This Entity Please Check Your Active And Deactive Configurations.", "Configuration Entity Is Not Valid !");

                    } else {
                        Configuration.setAttributeValue(Configuration.Fields.EntityLogicalName, trimmedValue);
                    }
                });

            } else {

                Configuration.setAttributeValue(Configuration.Fields.EntityLogicalName, null);
                Configuration.showError("RLBM-EX-0000", "Entity Logical Name For This Configuration Not Valid. Please Check CRM Entities.", "Configuration Entity Is Not Valid !");
            }

        }, function (e) {
            console.error(e.error.message);
        });
    }
}

function formOnSave(executionContext) {
    // Form Save Before Validation Actions

}

///ASYNC METHODS///

async function checkHasMultipleConfiguration() {
    const entityLogicalName = Configuration.getAttributeValue(Configuration.Fields.EntityLogicalName);

    if (Configuration.getFormType() === FormTypes.Create && entityLogicalName) {
        // Check if any configuration exists
        const query = `?$select=${Configuration.Fields.EntityLogicalName}&$filter=${Configuration.Fields.EntityLogicalName} eq '${entityLogicalName}'&$top=1`;
        const result = await Configuration.retrieveMultipleAsync(query);
        return result.entities && result.entities.length >= 1;
    }

    return false;
}
