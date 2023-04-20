// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const loadPizzas = filter => getPizzas(filter).then(renderPizzas);

const getPizzas = nome => axios.get('/api/pizzas', nome ? { params: { nome } } : {}).then(res => res.data);

const renderPizzas = pizzas => {
    const noPizzas = document.querySelector("#no-pizzas");
    const loader = document.querySelector("#pizzas-loader");
    const pizzasTbody = document.querySelector("#pizzas");
    const pizzasTable = document.querySelector("#pizzas-table");
    const pizzaFilter = document.querySelector("#pizzas-filter");

    if (pizzas && pizzas.length > 0) {
        pizzasTable.classList.add("d-block");
        pizzaFilter.classList.add("d-block");
        noPizzas.classList.remove("d-block");
    }
    else { noPizzas.classList.add("d-block"); }

    loader.classList.add("d-none");

    pizzasTbody.innerHTML = pizzas.map(pizzaComponent).join('');
};

const pizzaComponent = pizza => `
    <tr>
        <td>${pizza.id}</td>
        <td>${pizza.nome}</td>
        <td>${pizza.descrizione}</td>
        <td>${pizza.prezzo}</td>
    </tr>`;


const initFilter = () => {
    const filter = document.querySelector("#pizzas-filter input");
    filter.addEventListener("input", (e) => loadPizzas(e.target.value))
};