
const todoList = document.getElementById('todoList');
const statusFilter = document.getElementById('statusFilter');
const todoModal = new bootstrap.Modal(document.getElementById('todoModal'));
const deleteModal = new bootstrap.Modal(document.getElementById('deleteModal'));


let todos = [];
let currentTodoId = null;
let isEditMode = false;


document.addEventListener('DOMContentLoaded', () => {
    loadTodos();
    statusFilter.addEventListener('change', loadTodos);
});

async function loadTodos() {
    try {
        const status = statusFilter.value;
        let url = '/api/todos/GetTodos';

        if (status !== 'all') {
            url = `/api/todos/GetTodosByStatus/${status}`;
        }

        console.log("Calling:", url);

        const response = await fetch(url);
        console.log("Response status:", response.status);

        if (!response.ok) throw new Error('Failed to fetch todos');

        todos = await response.json();
        renderTodos();
    } catch (error) {
        console.error('Error loading todos:', error);
        alert('Failed to load todos. Please try again.');
    }
}

function renderTodos() {
    todoList.innerHTML = todos.map(todo => `
        <div class="list-group-item d-flex justify-content-between align-items-center 
            priority-${getPriorityCss(todo.priority)} 
            ${todo.status === 'Completed' ? 'completed' : ''}">
            
            <!-- Todo Info Area -->
            <div class="flex-grow-1 cursor-pointer" onclick="editTodo('${todo.id}')">
                <h5>${todo.title}</h5>
                ${todo.description ? `<p class="mb-1">${todo.description}</p>` : ''}
                ${todo.dueDate ? `<small>Due: ${new Date(todo.dueDate).toLocaleDateString()}</small>` : ''}
            </div>

            <!-- Action Buttons -->
            <div class="d-flex align-items-center">
               
                
                <!-- ✏️ Edit Button -->
                <button class="btn btn-sm btn-outline-primary me-1" onclick="editTodo('${todo.id}', event)">
                    <i class="bi bi-pencil"></i>
                </button>
                
                <!-- 🗑️ Delete Button -->
                <button class="btn btn-sm btn-outline-danger" onclick="showDeleteModal('${todo.id}', event)">
                    <i class="bi bi-trash"></i>
                </button>
            </div>
        </div>
    `).join('');
}


function getPriorityCss(priority) {
    switch (priority) {
        case 0: return 'low';
        case 1: return 'medium';
        case 2: return 'high';
        default: return 'low';
    }
}

function getStatusBadgeColor(status) {
    switch (status) {
        case 'Completed': return 'success';
        case 'InProgress': return 'warning';
        default: return 'secondary';
    }
}

function showAddModal() {
    isEditMode = false;
    document.getElementById('modalTitle').textContent = 'Add New Todo';
    document.getElementById('todoForm').reset();
    document.getElementById('todoId').value = '';
    todoModal.show();
}

function editTodo(id) {
    const todo = todos.find(t => t.id === id);
    if (!todo) return;

    isEditMode = true;
    currentTodoId = id;
    document.getElementById('modalTitle').textContent = 'Edit Todo';
    document.getElementById('todoId').value = todo.id;
    document.getElementById('title').value = todo.title;
    document.getElementById('description').value = todo.description || '';
    document.getElementById('priority').value = todo.priority;
    document.getElementById('dueDate').value = todo.dueDate ? todo.dueDate.split('T')[0] : '';
    todoModal.show();
}

async function saveTodo() {
    const form = document.getElementById('todoForm');
    if (!form.checkValidity()) {
        form.classList.add('was-validated');
        return;
    }

    const todoData = {
        title: document.getElementById('title').value,
        description: document.getElementById('description').value,
        priority: parseInt(document.getElementById('priority').value),
        dueDate: document.getElementById('dueDate').value || null
    };

    const url = isEditMode
        ? `/api/todos/UpdateTodo?id=${currentTodoId}`
        : '/api/todos/CreateTodo';

    const method = isEditMode ? 'PUT' : 'POST';

    try {
        console.log("Sending:", JSON.stringify(todoData));

        const response = await fetch(url, {
            method,
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(todoData)
        });

        if (!response.ok) {
            const errText = await response.text();
            throw new Error(`Save failed: ${errText}`);
        }

        todoModal.hide();
        await loadTodos();
        showAlert(`Todo ${isEditMode ? 'updated' : 'created'} successfully!`, 'success');
    } catch (error) {
        console.error('Error saving todo:', error);
        showAlert('Failed to save todo. Please try again.', 'danger');
    }
}

function showDeleteModal(id, event) {
    event.stopPropagation();
    currentTodoId = id;
    deleteModal.show();
}

async function confirmDelete() {
    try {
        const response = await fetch(`/api/todos/DeleteTodo/${currentTodoId}`, {
            method: 'DELETE'
        });

        if (!response.ok) throw new Error('Failed to delete todo');

        deleteModal.hide();
        await loadTodos();  
        showAlert('Todo deleted successfully!', 'success');
    } catch (error) {
        console.error('Error deleting todo:', error);
        showAlert('Failed to delete todo. Please try again.', 'danger');
    }
}


function showAlert(message, type) {
    const alertDiv = document.createElement('div');
    alertDiv.className = `alert alert-${type} alert-dismissible fade show position-fixed top-0 end-0 m-3`;
    alertDiv.innerHTML = `
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    `;
    document.body.appendChild(alertDiv);

    setTimeout(() => {
        alertDiv.classList.remove('show');
        setTimeout(() => alertDiv.remove(), 150);
    }, 3000);
}
