// JavaScript source code
//Api Variables
const baseUrl = "https://api.url.com"
const fetchDataUrl = baseUrl + "/queue/querybyconfiguration"
const fetchDeleteUrl = baseUrl + "/queue/delete/{0}"
const tokenUrl = baseUrl + "/identity/loginwithkey"
const urlParams = new URLSearchParams(window.location.search);
const paramsData = urlParams.get('data')
const paramsObject = JSON.parse(paramsData);
const guidEmpty = "00000000-0000-0000-0000-000000000000";
const Xrm = window.parent.Xrm;
const apiKey = paramsObject.apiKey
const apiKeyId = paramsObject.apiKeyId
const configurationId = paramsObject.configurationId


const fetchPageData = (page, authToken) => {
    const requestData = {
        ConfigurationId: configurationId,
        Page: page,
        Count: itemsPerPage,
    };
    fetch(fetchDataUrl, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${authToken}`,
        },
        body: JSON.stringify(requestData),
    })
        .then(response => response.json())
        .then(data => {
            backUpData = data.result.Data;
            updateTable(data.result.Data);
            updatePagination(data.result.TotalRecordCount);
            loadingSpinner.style.setProperty('display', 'none', 'important');
        })
        .catch(error => {
            console.error('Error fetching data:', error);
            loadingSpinner.style.setProperty('display', 'none', 'important');
        });
};

function fetchAuthToken() {
    return fetch(tokenUrl, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'ApiKey': apiKey,
        },
    })
        .then(response => response.json())
        .then(data => {
            authTokenCache = data.result; // Önbellekteki tokenı güncelle
            return data.result; // Tokenı döndür
        })
        .catch(error => {
            console.error('Error fetching data:', error);
            loadingSpinner.style.setProperty('display', 'none', 'important');
        });
}


function fetchDeleteHistory(recordId, authToken) {
    return fetch(fetchDeleteUrl.replace('{0}', recordId), {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${authToken}`
        },
    })
        .then(response => {
            return response.status === 204
        })
        .catch(error => {
            console.error('Error fetching data:', error);
            loadingSpinner.style.setProperty('display', 'none', 'important');
            return false;
        });
}


function createRollBackRequest(historyId, log) {

    const data = `{"configurationId": "${configurationId}","historyId": "${historyId}"}`

    let rollBackData =
    {
        "cr70c_requesttype": 2,
        "cr70c_configurationid@odata.bind": "/cr70c_rollbackmanagerlabconfigurations(" + configurationId + ")",
        "cr70c_apikeyid@odata.bind": "/cr70c_rollbackmanagerapikeies(" + apiKeyId + ")",
        "cr70c_data": data,
        "cr70c_log": log,
    }

    return Xrm.WebApi.createRecord("cr70c_rollbackmanagerlabrequest", rollBackData).then(
        function success(result) {
            return result.id;
        }, function (error) {
            console.error(error.message);
            return guidEmpty;
        })

}


function getUserSettings() {
    const userSettings = Xrm.Utility.getGlobalContext().userSettings;
    const settings = `{"userId": "${userSettings.userId}","userName": "${userSettings.userName}"}`
    return JSON.parse(settings);
}