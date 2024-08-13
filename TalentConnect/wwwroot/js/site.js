document.getElementById("CEP").onchange = function () {
    var cep = document.getElementById("CEP").value;
    var xhr = new XMLHttpRequest();
    xhr.open("GET", "https://viacep.com.br/ws/" + cep + "/json/");
    xhr.onload = function () {
        var response = JSON.parse(xhr.responseText);
        document.getElementById("Bairro").value = response.bairro;
        document.getElementById("Cidade").value = response.localidade;
        document.getElementById("Estado").value = response.uf;
    };
    xhr.send();
};


document.getElementById('add-habilidade').addEventListener('click', function () {
        var container = document.getElementById('habilidades-container');
        var div = document.createElement('div');
        div.className = 'form-group';
        var input = document.createElement('input');
        input.type = 'text';
        input.name = 'Habilidades[]';
        input.className = 'form-control';
        div.appendChild(input);
        container.appendChild(div);
    });

