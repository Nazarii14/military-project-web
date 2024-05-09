document.addEventListener('DOMContentLoaded', function () {
    const usersTab = document.getElementById('usersTab');
    const brigadesTab = document.getElementById('brigadesTab');
    const requestsTab = document.getElementById('requestsTab');

    const usersContent = document.getElementById('users_content');
    const brigadesContent = document.getElementById('brigades_content');
    const requestsContent = document.getElementById('requests_content');

    usersContent.style.display = 'block';
    brigadesContent.style.display = 'none';
    requestsContent.style.display = 'none';

    usersTab.addEventListener('click', function () {
        usersContent.style.display = 'block';
        brigadesContent.style.display = 'none';
        requestsContent.style.display = 'none';

        usersTab.classList.add('active-tab');
        brigadesTab.classList.remove('active-tab');
        requestsTab.classList.remove('active-tab');
    });

    brigadesTab.addEventListener('click', function () {
        usersContent.style.display = 'none';
        brigadesContent.style.display = 'block';
        requestsContent.style.display = 'none';

        usersTab.classList.remove('active-tab');
        brigadesTab.classList.add('active-tab');
        requestsTab.classList.remove('active-tab');
    });

    requestsTab.addEventListener('click', function () {
        usersContent.style.display = 'none';
        brigadesContent.style.display = 'none';
        requestsContent.style.display = 'block';

        usersTab.classList.remove('active-tab');
        brigadesTab.classList.remove('active-tab');
        requestsTab.classList.add('active-tab');
    });
});