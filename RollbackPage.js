// JavaScript source code

//Pagination Variables
const loadingSpinner = document.getElementById('loadingSpinner');
const itemsPerPage = 10; // Sayfa başına gösterilen eleman sayısı
let currentPage = 1; // Başlangıçta gösterilen sayfa
const maxVisiblePages = 5; // Sayfa numaralarındaki maksimum görünür sayı

//In-App Cache Variables
let backUpData = [];
let authTokenCache = "";

function pageOnload() {

    loadingSpinner.style.setProperty('display', 'block', 'important');

    getAuthToken()
        .then(token => {
            // Token başarıyla alındı
            fetchPageData(currentPage, token);
        })
        .catch(error => {
            // Hata durumu
            console.error("Error fetching token or page data:", error);
            loadingSpinner.style.setProperty('display', 'none', 'important');
        });
}
function getAuthToken() {
    if (authTokenCache === "" || isTokenExpired(authTokenCache)) {
        // Önbellekte token yoksa veya süresi dolmuşsa, yeni bir token iste
        return fetchAuthToken();
    } else {
        // Önbellekte geçerli bir token varsa, bir Promise ile döndür
        return Promise.resolve(authTokenCache);
    }
}
function updateTable(data) {
    const dataTable = document.getElementById('data-table');
    dataTable.innerHTML = `
                                    <thead class="thead-dark">
                                        <tr>
                                            <th scope="col">Operation Date</th>
                                            <th scope="col">Operation Type</th>
                                            <th scope="col">Execution Owner Id (GUID)</th>
                                            <th scope="col">Entity Logical Name</th>
                                            <th scope="col">Record Id (GUID)</th>
                                            <th scope="col">Details</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        ${data.map(item => `
                                            <tr>
                                                <th scope="row">${new Date(item.operationDate).toLocaleString()}</th>
                                                <td>${item.operationType == 2 ? 'Update' : 'Delete'}</td>
                                                <td>${item.executionOwnerId}</td>
                                                <td>${item.logicalName}</td>
                                                <td>${item.recordUniqueIdentifier}</td>
                                                <td>
                                                <button type="button" data-toggle="modal" data-target="#attributesModal" class="btn btn-dark">Attributes</button>
                                                <input type="hidden" id="hiddenId" value="${item.id}">
                                                </td>
                                            </tr>`).join('')}
                                    </tbody>
                                `;
}

function updatePagination(totalRecords) {
    const paginationContainer = document.getElementById('pagination-container');
    paginationContainer.innerHTML = '';

    const totalPages = Math.ceil(totalRecords / itemsPerPage);

    let startPage = Math.max(1, currentPage - Math.floor(maxVisiblePages / 2));
    let endPage = Math.min(startPage + maxVisiblePages - 1, totalPages);

    if (startPage > 1) {
        addPaginationItem('«', currentPage - 1);
    }

    for (let i = startPage; i <= endPage; i++) {
        addPaginationItem(i, i);
    }

    if (endPage < totalPages) {
        addPaginationItem('»', currentPage + 1);
    }
}

function addPaginationItem(label, pageNumber) {
    const paginationContainer = document.getElementById('pagination-container');
    const listItem = document.createElement('li');
    const pageLink = document.createElement('a');
    pageLink.href = '#';
    pageLink.className = 'page-link text-dark';
    pageLink.textContent = label;

    pageLink.addEventListener('click', function () {
        currentPage = pageNumber;
        pageOnload();
    });

    listItem.appendChild(pageLink);
    paginationContainer.appendChild(listItem);
}

function createAttributesTable(itemId) {
    const tableBody = document.getElementById('attributesTableBody');
    const data = backUpData.find(x => x.id == itemId);

    let tableHTML = '<table class="table"><thead class="thead-dark"><tr><th>Type</th><th>Name</th><th>Value</th></tr></thead><tbody>';

    data.attributes.forEach(item => {
        tableHTML += `<tr><td>${item.type}</td><td>${item.name}</td><td>${escapeXml(item.value)}</td></tr>`;
    });

    tableHTML += '</tbody></table>';
    tableBody.innerHTML = tableHTML;

}

function escapeXml(unsafe) {
    return unsafe
        .replace(/[<>&'"]/g, char => {
            switch (char) {
                case '<': return '&lt;';
                case '>': return '&gt;';
                case '&': return '&amp;';
                case '\'': return '&apos;';
                case '"': return '&quot;';
                default: return char;
            }
        });
}

function confirmDelete() {
    // Kullanıcıya onay mesajı göster
    const isConfirmed = confirm("Are you sure you want to delete?");
    const recordId = document.getElementById('hiddenIdModal').value;
    // Kullanıcının seçimine göre işlem yap
    if (isConfirmed) {

        getAuthToken()
            .then(token => {
                fetchDeleteHistory(recordId, token)
                    .then(statu => {
                        if (statu) {
                            alert("Deletion is successfull !");
                        } else {
                            alert("Deletion is unsuccessfull !");
                        }
                        location.reload()
                    }).catch(error => {
                        // Hata durumu
                        console.error("Error deleting data:", error);
                        loadingSpinner.style.setProperty('display', 'none', 'important');
                    });
            }).catch(error => {
                // Hata durumu
                console.error("Error fetching token or page data:", error);
                loadingSpinner.style.setProperty('display', 'none', 'important');
            });

    } else {
        // Kullanıcı 'Hayır' dediyse bir şey yapma veya iptal et
        alert("Deletion canceled!");
    }
}

function confirmRollback() {
    // Kullanıcıya onay mesajı göster
    const isConfirmed = confirm("Are you sure you want to rollback?");
    const recordId = document.getElementById('hiddenIdModal').value;
    const userInfo = getUserSettings();
    const date = new Date();
    const log = `${date.toUTCString()}, rollback request created by ${userInfo.userName}, User Id: ${userInfo.userId}.`;
    // Kullanıcının seçimine göre işlem yap
    if (isConfirmed) {
        createRollBackRequest(recordId, log).then(id => {
            if (id != guidEmpty) {
                alert("Rollback operation completed! Request Id: " + id);
            } else {
                alert("Rollback operation not completed! Request Id: " + id);
            }
            location.reload()
        }).catch(error => {
            // Hata durumu
            console.error("Error creating rollback data:", error);
            loadingSpinner.style.setProperty('display', 'none', 'important');
        });
    } else {
        alert("Rollback operation canceled!");
    }
}

function isTokenExpired(jwtToken) {
    const tokenParts = jwtToken.split('.');
    if (tokenParts.length !== 3) {
        throw new Error('Invalid JWT format');
    }

    const payload = JSON.parse(atob(tokenParts[1]));
    if (!payload.exp) {
        throw new Error('Expiration date not found in the token');
    }

    const currentTimestamp = Math.floor(new Date().getTime() / 1000); // Şu anki zamanın Unix timestamp'i
    return payload.exp < currentTimestamp; // Expire tarihi geçmişse true döner
}

//EVENTS
$('#attributesModal').on('show.bs.modal', function (e) {
    // Tıklanan butondaki gizli ID değerini alın
    const clickedButton = e.relatedTarget;
    const itemId = $(clickedButton).siblings('input[type="hidden"]').val();
    $('#hiddenIdModal').val(itemId);
    // Modal içeriğini oluşturan fonksiyonu çağırın
    createAttributesTable(itemId);
});