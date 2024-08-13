function toggleDataFim() {
    var checkBox = document.getElementById("Cursando");
    var dataFim = document.getElementById("DataFim");
    if (checkBox.checked == true) {
        dataFim.disabled = true;
    } else {
        dataFim.disabled = false;
    }
}

function addFormacaoEntry() {
    var container = document.getElementById('formacaoContainer');
    var newEntry = container.querySelector('.formacaoEntry').cloneNode(true);
    newEntry.querySelectorAll('input').forEach(input => input.value = '');
    container.appendChild(newEntry);
}

function removeFormacaoEntry(button) {
    var container = document.getElementById('formacaoContainer');
    if (container.childElementCount > 1) {
        button.parentElement.remove();
    }
}

function addExperienciaEntry() {
    var container = document.getElementById('experienceContainer');
    var newEntry = container.querySelector('.experience-item').cloneNode(true);

    // Limpa os valores dos campos de entrada
    newEntry.querySelectorAll('input').forEach(input => input.value = '');

    // Atualiza o título da nova entrada para refletir o número correto
    var experienceCount = container.querySelectorAll('.experience-item').length + 1;
    newEntry.querySelector('h5').textContent = `Experiência ${experienceCount}`;

    // Adiciona a nova entrada ao contêiner
    container.appendChild(newEntry);
}
