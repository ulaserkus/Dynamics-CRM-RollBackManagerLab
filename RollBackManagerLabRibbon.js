// JavaScript source code

function openRollBackPage() {

    const configurationId = Xrm.Page.data.entity.getId();

    if (configurationId != '') {
        const apiKey = Xrm.Page.getAttribute("cr70c_rollbackmanagerlabapikey").getValue()[0].name;
        const apiKeyId = Xrm.Page.getAttribute("cr70c_rollbackmanagerlabapikey").getValue()[0].id;
        const customParameters = `{
                                    "configurationId": "${configurationId.replace("{", "").replace("}", "")}",
                                    "apiKey": "${apiKey}",
                                    "apiKeyId": "${apiKeyId.replace("{", "").replace("}", "")}"
                                  }`;
        // Open web resource
        Xrm.Navigation.navigateTo(
            { pageType: "webresource", webresourceName: "cr70c_RollbackPage.html", data: customParameters },
            { target: 2, position: 1, width: { value: 95, unit: "%" } },
        );

    } else {
        // Error message would go here - case must be saved first.
    }
}