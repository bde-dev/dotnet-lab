const uri = 'api/todoitems';
let todoItems = [];

function GetItems() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}

function AddItem() {
    const addNameTextBox = document.getElementById('add-name');

    const item = {
        isComplete: false,
        Name: addNameTextBox.value.trim() 
    }

    fetch(uri, {
        method: "POST",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then(() => {
            GetItems();
            addNameTextBox.value = " ";
        })
        .catch(error => consol.error('Unable to add item.', error));
}

function DeleteItem(id) {
    fetch(`${uri}/{id}`, {
        method: 'DELETE'
    })
        .then(() => GetItems())
        .catch(error => console.error('Unable to delete item.', error));
}

function DisplayEditForm(id) {
    const item = todoItems.find(item => item.id == id);

    document.getElementById('edit-name').value = item.Name;
    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-isComplete').checked = item.isComplete;
    document.getElementById('editForm').style.display = 'block';
}

function UpdateItem() {
    const itemId = document.getElementById('edit-id').value;
    const item = {
        id: parseInt(itemId, 10),
        isComplete: document.getElementById('edit-isComplete').checked,
        name: document.getElementById('edit-name').value.trim()
    };

    fetch(`${uri}/${itemId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(() => GetItems())
        .catch(error => console.error('Unable to update item.', error));

    CloseInput();

    return false;
}

function CloseInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayCount(itemCount) {
    const name = (itemCount === 1) ? 'to-do' : 'to-dos';

    document.getElementById('counter').innerText = `${itemCount} ${name}`;
}

function _displayItems(data) {
    const tbody = document.getElementById('todos');
    tbody.innerHTML = '';

    _displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(item => {
        let isCompleteCheckbox = document.createElement('input');
        isCompleteCheckbox.type = 'checkbox';
        isCompleteCheckbox.disabled = true;
        isCompleteCheckbox.checked = item.isComplete;

        let editButton = button.cloneNode(false);
        editButton.innerText = 'edit';
        editButton.setAttribute('onclick', `DisplayEditForm(${item.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'delete';
        deleteButton.setAttribute('onclick', `DisplayEditForm(${item.id})`);

        let tr = tbody.insertRow();

        let td1 = tr.insertCell(0);
        td1.appendChild(isCompleteCheckbox);

        let td2 = tr.insertCell(1);
        let textNode = document.createTextNode(item.name);
        td2.appendChild(textNode);

        let td3 = tr.insertCell(2);
        td3.appendChild(editButton);

        let td4 = tr.insertCell(3);
        td4.appendChild(deleteButton);
    });

    todoItems = data;
}