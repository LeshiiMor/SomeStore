$(document).ready(function () {
    $.ajax({
        type: 'get',
        url: '/Cart/GetAmountItems',
        success: function (amount) {
            $('#amountBasketItBadge').text(amount)
        },
        error: function (req, status, error) {
            console.log(req.status)
            console.log(req.responseText)
        }
    })
})
$('#text-welcome').show(1000);

function updatePicture(input)
{
    let file = input.files[0];
    $('#prodImg').attr('src', URL.createObjectURL(file))
    $('#paperIcon').hide(500)
    $('#completeIcon').show(1000)
    $('#prodImg').show(1000)
    $('#labelfile-text').text('Изменить фото')
}
function LoadFiltInfo() {
    let category = $('#categorySelect').val();
    let name = $('#inp-namesearch').val();
    $.ajax({
        type: 'get',
        url: '/Product/GetFiltrProduct',
        data: { 'name': name, 'category': category },
        success: function (msg) {
            if (msg.length === 0) {
                $('.product-container').html('<h3>Пусто</h3>')
            }
            else {
                $('.product-card').fadeToggle(300)
                setTimeout(() => { $('.product-card').remove() }, 300)
                setTimeout(() => {
                    $('.product-container').html('')
                    for (let i = 0; i < msg.length; i++) $('.product-container').append(ProductCardBuilder(msg[i]))
                    $('.product-card').fadeToggle();
                },300)
            }
        },
        error: function (req,status,error) {
            console.log(error)
        }
    })
}
function ProductCardBuilder(product)
{
    let ProductCard = document.createElement('div')
    let div = document.createElement('div')
    let img = document.createElement('img')
    div.innerHTML = '<h4 class="h4-product">' + product.name + '</h4>' +
        '<p class="mt-3" > <strong>' + product.price + '&#36;</strong></p >' +
        '<input name="product_id" type="hidden" value="' + product.id + '" />'+
        '<a href="/Product/ProductCard/' + product.id + '">Подробнее</a>'

    div.classList.add('product-card-infodiv')
    img.classList.add('img-fluid')
    img.classList.add('img-product')
    img.setAttribute('src', product.imageURL)
    img.setAttribute('title', product.Name)
    ProductCard.classList.add('product-card')
    ProductCard.insertAdjacentElement("beforeend", img)
    ProductCard.insertAdjacentElement("beforeend", div)
    ProductCard.classList.add('hide')
    return ProductCard;
}
function Test() {
    console.log('Test')
}
function Test(message) {
    alert(message)
}
function CloseModal() {
    $("#staticBackdrop").modal('hide');
}
function OpenModal() {
    $('#requestInfo').hide();
    $("#staticBackdrop").modal();
}
function AddCategoryFromModal() {
    let nameCategory = $('#modal-input-nameCat').val()
    if (nameCategory.length === 0) {
        $('#requestInfo').addClass("bg-danger")
        $('#requestInfo').text('Заполните поле')
        if ($('#requestInfo').is(':hidden')) {
            $('#requestInfo').show(500)
        }
    }
    else {
        $.ajax({
            type: 'post',
            url: '/Category/AddCategory',
            data: { 'nameCategory': nameCategory },
            success: function (msg) {
                $('#requestInfo').addClass("bg-success")
                $('#requestInfo').removeClass("bg-danger")
                $('#requestInfo').text(msg)
                $.ajax({
                    type: 'get',
                    url: '/Category/GetAllCategory',
                    success: function (msg) {
                        $('#categoriesSelect').find('option').remove()
                        for (let i = 0; i < msg.length; i++) {
                            $('#categoriesSelect').append(`<option value="${msg[i].id}">${msg[i].name}</option>`)
                        }
                    },
                    error: function (req, status, error) {
                        console.log(error)
                    }
                })
            },
            error: function (req, status, error) {
                $('#requestInfo').addClass("bg-danger")
                $('#requestInfo').removeClass("bg-success")
                $('#requestInfo').text(req.responseText)
            }
        })
        if ($('#requestInfo').is(':hidden')) {
            $('#requestInfo').show(500)
        }
    }
}
function RegistrationUser()
{
    if ($('#rePasInput').is(':hidden')) {
        $('#rePasInput').show(1000)
        $('#btn-backtolog').show(1000);
        $('#logBtn').hide(1000)
    }
    else {
        let login = $('#loginInput').val();
        let pas = $('#pasInput').val();
        let rePas = $('#rePasInput').val();
        if (login.length != 0 && pas.length != 0 && rePas.length != 0) {
            if ($('#loginInput').hasClass('valid-error')) $('#loginInput').removeClass('valid-error')
            if ($('#pasInput').hasClass('valid-error')) $('#pasInput').removeClass('valid-error')
            if ($('#rePasInput').hasClass('valid-error')) $('#rePasInput').removeClass('valid-error')
            if ($('#message-box').is(':hidden') != true) $('#message-box').hide(500)
            if (pas === rePas) {
                $.ajax({
                    type: 'post',
                    url: '/Account/Register',
                    data: { 'UserName': login, 'Password': pas, 'ReentryPassword': rePas },
                    success: function (msg) {
                        $('.login-block').fadeOut(500)
                        setTimeout(() => { $('.login-block').empty() },500)
                        setTimeout(() => { $('.login-block').append(msg) },500)
                        setTimeout(() => { $('.login-block').fadeIn(500) },500)
                    },
                    error: function (req, status, error) {
                        $('#message-box').show(500).text(req.responseText)
                    }
                })
            }
            else {
                $('#rePasInput').addClass('valid-error')
                $('#pasInput').addClass('valid-error')
                $('#message-box').show(500).text('Пароли не совпадают')
            }
        }
        if (login.length === 0) {
            $('#loginInput').addClass('valid-error')
            $('#message-box').show(500).text('Заполнены не все поля')
        }
        if (pas.length === 0) {
            $('#pasInput').addClass('valid-error')
            $('#message-box').show(500).text('Заполнены не все поля')
        }
        if (rePas.length === 0) {
            $('#rePasInput').addClass('valid-error')
            $('#message-box').show(500).text('Заполнены не все поля')
        }
    }
}
function ShowLoginUser()
{
    $('#rePasInput').hide(1000)
    $('#btn-backtolog').hide(1000);
    $('#logBtn').show(1000)
    $('#message-box').hide(500)
    if ($('#loginInput').hasClass('valid-error')) $('#loginInput').removeClass('valid-error')
    if ($('#pasInput').hasClass('valid-error')) $('#pasInput').removeClass('valid-error')
    if ($('#rePasInput').hasClass('valid-error')) $('#rePasInput').removeClass('valid-error')
    if ($('#message-box').is(':hidden') != true) $('#message-box').hide(500)
}
$('#logBtn').click(function () {
    let login = $('#loginInput').val();
    let pas = $('#pasInput').val();
    if (login.length != 0 && pas.length != 0) {
        $('#signinform').submit()
    }
    else {
        if (login.length === 0) {
            $('#loginInput').addClass('valid-error')
            $('#message-box').show(500).text('Заполнены не все поля')
        }
        if (pas.length === 0) {
            $('#pasInput').addClass('valid-error')
            $('#message-box').show(500).text('Заполнены не все поля')
        }
    }
})
function RedirectPage(path)
{
    window.location = path
}
function AddMoreInfoUser() {
    var inputs = $('#moreInfoUser :input')
    inputs.each(function () {
        if ($(this).val().length === 0 && $(this).prop("tagName") === 'INPUT') {
            $(this).addClass('valid-error')
        }
        else {
            $(this).removeClass('valid-error')
        }
    })
    if ($('.valid-error').length === 0) {
        $.ajax({
            type: 'post',
            url: '/Account/AddMoreInfo',
            data: { 'name': inputs[0].value, 'surname': inputs[1].value, 'patronomic': inputs[2].value, 'phone': inputs[3].value },
            success: function (msg) {
                RedirectPage('/')
            },
            error: function (req, status, error) {
                console.log('все плохо ->' + req.responseText)
            }
        })
    }
}

$('.cab-link').click(function () {
    let link = $(this)[0]
    let links = $('.cab-link')
    for (let i = 0; i < links.length; i++) {
        if (link == links[i]) {
            links[i].classList.add('cab-active')
            GetCabinetView(links[i].getAttribute('data-href'))
        }
        else { links[i].classList.remove('cab-active') }
    }
})
function GetCabinetView(url) {
    $.ajax({
        type: 'get',
        url: url,
        success: function (msg) {
            $('.cab-right').html(msg)
        },
        error: function (req, status, error) {
            console.error(req)
            console.error(error)
        }
    })
}
function EditPhone() {
    let value = $('#inp-phone').val()
    value = value.replace(/\s+/g, '')
    if (value.length != 12) {
        $(this).addClass('valid-error')
    }
    else if (isNaN(value)) {
        $(this).addClass('valid-error')
    }
    else {
        $(this).removeClass('valid-error')
        let truePhoneNumber = '+'
        for (let i = 0; i < value.length; i++) {
            if (i == 3) truePhoneNumber += '('
            else if (i == 5) truePhoneNumber += ')'
            else if (i == 8 || i == 10) truePhoneNumber += '-'
            truePhoneNumber += value[i]
        }
        $(this).val(truePhoneNumber)
    }
}

$('#inp-phone').on("change", function ()
{
    let value = $('#inp-phone').val()
    value = value.replace(/\s+/g, '')
    if (value.length != 12)
    {
        $(this).addClass('valid-error')
    }
    else if (isNaN(value))
    {
        $(this).addClass('valid-error')
    }
    else
    {
        $(this).removeClass('valid-error')
        let truePhoneNumber = '+'
        for (let i = 0; i < value.length; i++)
        {
            if (i == 3) truePhoneNumber += '('
            else if (i == 5) truePhoneNumber += ')'
            else if (i == 8 || i == 10) truePhoneNumber += '-'
            truePhoneNumber += value[i]
        }
        $(this).val(truePhoneNumber)
    }
})

function UpdatePersonalInfo()
{
    let email = $('#inp-email').val()
    let name = $('#inp-name').val()
    let surname = $('#inp-surname').val()
    let patron = $('#inp-patron').val()
    let phone = $('#inp-phone').val()

    if (email.length > 0 && !validateEmail(email)) {
        $('#inp-email').addClass('valid-error')
    }
    else $('#inp-email').removeClass('valid-error')

    if (name.length == 0) {
        $('#inp-name').addClass('valid-error')
    }
    else $('#inp-name').removeClass('valid-error')
    if (surname.length == 0) {
        $('#inp-surname').addClass('valid-error')
    }
    else $('#inp-surname').removeClass('valid-error')
    if (patron.length == 0) {
        $('#inp-patron').addClass('valid-error')
    }
    else $('#inp-patron').removeClass('valid-error')

    if ($('.valid-error').length > 0) {
        $('#message-box').addClass('bg-danger').text('Заполните отмеченные поля корректно').show(500);
    }
    else {
        $('#message-box').hide(200);
        $.ajax({
            type: 'post',
            url: '/Account/PersonalInfo',
            data: { 'Name': name, 'Surname': surname, 'Patronymic': patron, 'Email': email, 'PhoneNumber': phone },
            success: function (msg) {
                $('#message-box').removeClass('bg-danger')
                $('#message-box').addClass('bg-success').show(500).text(msg)
            },
            error: function (req, status, error) {
                console.error(req)
                console.error(error)
                $('#message-box').removeClass('bg-success')
                $('#message-box').addClass('bg-danger').show(500).text(req.responseText)
            }
        })

    }
}

function validateEmail(email) {
    const re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}

function ChangeLogin()
{
    if ($('.lognew-block').is(':hidden')) {
        $('.lognew-block').show(500)
        $('#btn-cancelchLog').show(500)
    }
    else {
        let newLogin = $('#inp-log-new').val();
        let pass = $('#inp-log-pas').val();
        let oldLogin = $('#inp-log').val();
        if (pass.length == 0 || newLogin == oldLogin) $('#inp-log-new').addClass('valid-error')
        else $('#inp-log-new').removeClass('valid-error')
        if (newLogin.length == 0) $('#inp-log-pas').addClass('valid-error')
        else $('#inp-log-pas').removeClass('valid-error')
        if (!CheckValid()) {
            $.ajax({
                type: 'post',
                url: '/Account/ChangeLogin',
                data: { 'newLogin': newLogin, 'password': pass },
                success: function (msg) {
                    ShowToast('success', msg)
                },
                error: function (req, status, error) {
                    ShowToast('error', req.responseText)
                    console.error(req)
                    console.error(error)
                }
            })
        }
    }
}
function CancelChangeLog() {
    $('#inp-log-new').val('')
    $('.lognew-block').hide(500)
    $('#btn-cancelchLog').hide(500)
}

function CheckValid() {
    return ($('.valid-error').length>0)
}
function ShowToast(type, message)//type 1.error 2.success
{
    switch (type) {
        case 'error': $('.toast').addClass('bg-danger'); break;
        case 'success': $('.toast').addClass('bg-success'); break;
    }
    $('.toast-body').text(message)
    var toastElList = [].slice.call(document.querySelectorAll('.toast'))
    var toastList = toastElList.map(function (toastEl) {
        return new bootstrap.Toast(toastEl)
    });
    toastList.forEach(toast => toast.show()); // This show them
}

function ChangePassword()
{
    let oldPass = $('#inp-pas-old').val();
    let newPass = $('#inp-pas-new').val();
    if (oldPass.length == 0 || oldPass == newPass) $('#inp-pas-old').addClass('valid-error')
    else $('#inp-pas-old').removeClass('valid-error')
    if (newPass.length == 0) $('#inp-pas-new').addClass('valid-error')
    else $('#inp-pas-new').removeClass('valid-error')
    if (!CheckValid()) {
        $.ajax({
            type: 'post',
            url: '/Account/ChangePassword',
            data: { 'password': oldPass, 'newpassword': newPass },
            success: function (msg) {
                ShowToast('success', msg)
            },
            error: function (req, status, error) {
                ShowToast('error', req.responseText)
                console.error(req)
                console.error(error)
            }
        })
    }
}
$('.admin-nav-link').click(function () {
    let url;
    var links = $('.admin-nav-link')
    for (var i = 0; i < links.length; i++) {
        if (links[i] == $(this)[0]) {
            $(this)[0].parentElement.classList.add('cab-active')
            url = $(this)[0].getAttribute('data-href')
        }
        else links[i].parentElement.classList.remove('cab-active')
    }
    $.ajax({
        type: 'get',
        url: url,
        success: function (msg) {
            $('#admin-contentblock').html(msg)
            OpenModelUserInfo();
            OpenModelProductInfo();
        },
        error: function (req, status, error) {
            console.error(error)
            console.error(req.responseText)
            console.error(status)
        }
    })
})
function OpenModelUserInfo() {
    $('.table-row').click(function () {
        let row = $(this)[0]
        let idUser = row.children[0].getAttribute('data-id')
        if (idUser != 0) {
            $.ajax({
                    type: 'get',
                url: '/Manage/GetUser',
                    data: { 'id': idUser },
                    success: function (msg) {
                        let form = $('#admin-acc-modalform')[0]
                        $(form).find("input[name=name]").val(msg.name)
                        $(form).find("input[name=id]").val(msg.id)
                        $(form).find("input[name=surname]").val(msg.surname)
                        $(form).find("input[name=patron]").val(msg.patronymic)
                        $(form).find("input[name=email]").val(msg.email)
                        $(form).find("input[name=phone]").val(msg.phoneNumber)
                        $(form).find("input[id=" + msg.role.name + "]").prop('checked', true)
                        $('#span-username').text(msg.userName)
                    },
                    error: function (req, status, error) {
                        console.log(error)
                    }  
                })
            let modal = new bootstrap.Modal(document.querySelector('#exampleModal'));
            modal.show();
        }
        
    })
}
OpenModelUserInfo();

function DeleteUser() {
    let id = $('#admin-acc-modalform').find('input[name=id]').val()
    console.log(id)
    $.ajax({
        type: 'post',
        url: '/Manage/DeleteUser',
        data: { 'id': id },
        success: function (msg) {
            $('#message-box').text(msg).removeClass('bg-danger').addClass('bg-success').show(200)
            setTimeout(() => {
                $('#exampleModal').modal('hide');
            }, 300)
            setTimeout(() => {
                $.ajax({
                    type: 'get',
                    url: '/Manage/Accounts',
                    success: function (msg) {
                        $('#admin-contentblock').html(msg)
                        OpenModelUserInfo();
                    },
                    error: function (req, status, error) {
                        console.error(error)
                        console.error(req.responseText)
                        console.error(status)
                    }
                })
            },500)
        },
        error: function (req, status, error) {
            $('#message-box').text(req.responseText).removeClass('bg-success').addClass('bg-danger').show(500)
        }
    })
}
function UpdateUserInfo() {
    let form = $('#admin-acc-modalform')[0]
    let name=$(form).find("input[name=name]").val()
    let id=$(form).find("input[name=id]").val()
    let surname=$(form).find("input[name=surname]").val()
    let patron=$(form).find("input[name=patron]").val()
    let email=$(form).find("input[name=email]").val()
    let phone=$(form).find("input[name=phone]").val()
    let role = $(form).find("input[name=role]:checked").val();
    if (name.length == 0 || surname.length == 0 || patron.length == 0 || email.length == 0 || phone.length == 0) {
        $('#message-box').text('не все поля заполнены').removeClass('bg-success').addClass('bg-danger').show(500)
    }
    else {
        $('#message-box').hide(500).removeClass('bg-danger')
        $.ajax({
            type: 'post',
            url: '/Manage/UpdateUser',
            data: { 'Id': id, 'Name': name, 'Surname': surname, 'Patronymic': patron, 'PhoneNumber': phone, 'Email': email, 'RoleId': role },
            success: function (msg) {
                $('#message-box').text(msg).addClass('bg-success').remove('bg-danger').show(500)
                setTimeout(() => {
                    $('#exampleModal').modal('hide');
                }, 300)
                setTimeout(() => {
                    $.ajax({
                        type: 'get',
                        url: '/Manage/Accounts',
                        success: function (msg) {
                            $('#admin-contentblock').html(msg)
                            OpenModelUserInfo();
                        },
                        error: function (req, status, error) {
                            console.error(error)
                            console.error(req.responseText)
                            console.error(status)
                        }
                    })
                }, 500)
            },
            error: function (req, status, error) {
                $('#message-box').text(req.responseText).removeClass('bg-success').addClass('bg-danger').show(500)
            }
        })
    }
}
function LoadFiltUser() {
        $.ajax({
            type: 'get',
            url: '/Manage/GetFiltrusers',
            data: { 'username': $('.admin-acc-inputSearch').val(), 'idRole': $('.admin-acc-selectlist').val() },
            success: function (msg) {
                $('#table-body').html('')
                for (let i = 0; i < msg.length; i++) {
                    let tableElement = `<tr class="table-row">
                                            <th class="hide" data-id="${msg[i].id}"></th>
                                            <td>${msg[i].name}</td>
                                            <td>${msg[i].surname}</td>
                                            <td>${msg[i].patronymic}</td>
                                            <td>${msg[i].email}</td>
                                            <td>${msg[i].role.name}</td>
                                        </tr>`
                    $('#table-body').append(tableElement);
                }
            },
            error: function (req, status, error) {
                if (req.status == 404) {
                    $('#table-body').html(req.responseText);
                }
            }
        })
}
function OpenModelProductInfo() {
    $('.table-row-product').click(function () {
        let row = $(this)[0]
        let idprod = row.children[0].getAttribute('data-id')
        if (idprod != 0) {
            $.ajax({
                type: 'get',
                url: '/Product/GetProduct',
                data: { 'id': idprod },
                success: function (msg) {
                    let form = $('#admin-acc-modalform')[0]
                    $(form).find("input[name=name]").val(msg.name)
                    $(form).find("input[name=id]").val(msg.id)
                    $(form).find("input[name=dis]").val(msg.description)
                    $(form).find("input[name=price]").val(msg.price)
                    $(form).find("select").val(msg.category.id)
                    if (msg.isFavourite)
                        $(form).find("input[type=checkbox]").prop('checked', true)
                    else $(form).find("input[type=checkbox]").prop('checked', false)
                    $('#span-username').text(msg.id)
                },
                error: function (req, status, error) {
                    console.log(req.responseText)
                    console.log(req.status)
                }
            })

            let modal = new bootstrap.Modal(document.querySelector('#exampleModal'));
            modal.show();
        }

    })
}
OpenModelProductInfo();
function DeleteProduct() {
    let idProduct = $('#admin-acc-modalform').find('input[name=id]').val()
    if (idProduct != 0) {
        $.ajax({
            type: 'post',
            url: '/Product/Delete',
            data: { 'id': idProduct },
            success: function (msg) {
                $('#message-box').show(200).removeClass('bg-danger').addClass('bg-success').text(msg)
                setTimeout(() => { $('#exampleModal').modal('hide') }, 500)
                setTimeout(() => {
                    $.ajax({
                        type: 'get',
                        url: '/Manage/Products',
                        success: function (msg) {
                            $('#admin-contentblock').html(msg)
                            OpenModelProductInfo();
                        },
                        error: function (req, status, error) {
                            console.error(error)
                            console.error(req.responseText)
                            console.error(status)
                        }
                    })
                }, 800)
            },
            error: function (req, status, error) {
                if (req.status == 404) {
                    $('#message-box').show(500).removeClass('bg-success').addClass('bg-danger').text(req.responseText)
                }
            }
        })
    }
}
function UpdateProduct() {
    let form = $('#admin-acc-modalform')[0]
    let name=$(form).find("input[name=name]").val()
    let id = $(form).find("input[name=id]").val()
    let dis = $(form).find("input[name=dis]").val()
    let price = $(form).find("input[name=price]").val()
    let categoryId = $(form).find("select").val()
    let fav = $(form).find("input[type=checkbox]").prop('checked')
    if (name.length != 0 && dis.length != 0 && price.length != 0 && categoryId != 0 && id != 0) {
        $.ajax({
            type: 'post',
            url: '/Product/Update',
            data: { 'Id': id, 'Name': name, 'Description': dis, 'IsFavourite': fav, 'Price': price, 'CategoryId': categoryId },
            success: function (msg) {
                $('#message-box').show(200).removeClass('bg-danger').addClass('bg-success').text(msg)
                setTimeout(() => { $('#exampleModal').modal('hide') }, 500)
                setTimeout(() => {
                    $.ajax({
                        type: 'get',
                        url: '/Manage/Products',
                        success: function (msg) {
                            $('#admin-contentblock').html(msg)
                            OpenModelProductInfo();
                        },
                        error: function (req, status, error) {
                            console.error(error)
                            console.error(req.responseText)
                            console.error(status)
                        }
                    })
                }, 500)
            },
            error: function (req, status, error) {
                $('#message-box').show(500).removeClass('bg-success').addClass('bg-danger').text(req.responseText)
            }
        })
    }
}
function FiltrProductTable() {
    let category = $('.admin-acc-selectlist').val();
    let name = $('.admin-acc-inputSearch').val();
    $.ajax({
        type: 'get',
        url: '/Product/GetFiltrProduct',
        data: { 'name': name, 'category': category },
        success: function (msg) {
            if (msg.length === 0) {
                $('#table-body').html('<h3 class="text-center">Пусто</h3>')
            }
            else {
                $('#table-body').html('')
                for (let i = 0; i < msg.length; i++) {
                    let tableElement = `<tr class="table-row-product">
                                            <th class="hide" data-id="${msg[i].id}"></th>
                                            <td>${msg[i].name}</td>
                                            <td>${msg[i].category.name}</td>
                                            <td>${msg[i].price}</td>
                                        </tr>`
                    $('#table-body').append(tableElement);
                }
                OpenModelProductInfo();
            }
        },
        error: function (req, status, error) {
            console.log(error)
        }
    })
}

$('.btn-buy').click(function () {
    let idProd = $('#productCardID').val();
    if (idProd > 0) {
        $.ajax({
            type: 'post',
            url: '/Cart/AddProduct',
            data: { 'id': idProd },
            success: function (msg) {
                $('.btn-buy').hide(500)
                ShowToast('success', 'Товар добавлен в корзину')
                UpdateBadge('+')
            },
            error: function (req, status, error) {
                ShowToast('error', req.responseText)
            }
        })
    }
})
$('.cart-item-delbtn').click(function () {
    let btn = $(this)
    let idProd = btn.parent().parent().find('input[class=inp-cart-idProd]').val()
    $.ajax({
        type: 'post',
        url: '/Cart/DeleteItem',
        data: { 'prodId': idProd },
        success: function (msg) {
            $('div.row.cart-row[data-prodid=' + idProd + ']').hide(500)
            $('#finalsumm-span').text(msg)
            UpdateBadge('-')
        },
        error: function (req, status, error) {
            console.log(req.status)
            console.log(req.responseText)
        }
    })
})

$('.number-minus').click(function () {
    let inputNumber = $(this).parent().find('input[type=number]')
    let amount = parseInt($(this).parent().find('input[type=number]').val())
    if (amount > 1) {
        let idProd = $(this).parent().parent().parent().parent().find('input[hidden]').val();
        $.ajax({
            type: 'get',
            url: '/Product/GetProduct',
            data: { 'id': idProd },
            success: function (productJSON) {
                let oldValue = parseInt($('span[data-idprod=' + productJSON.id + ']').text())
                let currentSumm = parseInt($('span[id=finalsumm-span]').text());
                --amount;
                $('span[data-idprod=' + productJSON.id + ']').text(amount * productJSON.price)
                $(inputNumber).val(amount)
                currentSumm = currentSumm - oldValue
                currentSumm += amount * productJSON.price
                $('span[id=finalsumm-span]').text(currentSumm);

            },
            error: function (req, status, error) {
                console.log(req.status)
                console.log(req.responseText)
            }
        })
    }
})
$('.number-plus').click(function () {
    let inputNumber = $(this).parent().find('input[type=number]')
    let amount = parseInt($(this).parent().find('input[type=number]').val())
    let idProd = $(this).parent().parent().parent().parent().find('input[hidden]').val();
    $.ajax({
        type: 'get',
        url: '/Product/GetProduct',
        data: { 'id': idProd },
        success: function (productJSON) {
            let oldValue = parseInt($('span[data-idprod=' + productJSON.id + ']').text())
            let currentSumm = parseInt($('span[id=finalsumm-span]').text());
            ++amount;
            $('span[data-idprod=' + productJSON.id + ']').text(amount * productJSON.price)
            $(inputNumber).val(amount)
            currentSumm = currentSumm - oldValue
            currentSumm += amount * productJSON.price
            $('span[id=finalsumm-span]').text(currentSumm);
        },
        error: function (req, status, error) {
            console.log(req.status)
            console.log(req.responseText)
        }
    })
})
$('#acceptOrderBTN').click(function () {
    let orders = $('.cart-row')
    let ordersMap = []
    for (let i = 0; i < orders.length; i++) {
        let order = {
            ProductId: parseInt($(orders[i]).children().find('input[type=text]').val()),
            AmountProduct: parseInt($(orders[i]).children().find('input[type=number]').val())
        }
        ordersMap.push(JSON.stringify(order))
    }
    $.ajax({
        type: 'post',
        url: '/Cart/MakeOrder',
        data: { 'orderMap': ordersMap },
        success: function (msg) {
            $('.cart-row').hide(500)
            $('#finalsumm-span').text(0)
            UpdateBadge('',0)
            ShowToast('success', 'Заказ оформлен')
        },
        error: function (req, status, error) {
            ShowToast('error', req.responseText)
            console.log(req.responseText)
            console.log(req.status)
        }
    })
})

function UpdateBadge(symbol,currentNumber = -1) {
    let amount = $('#amountBasketItBadge').text()
    switch (symbol) {
        case '+': $('#amountBasketItBadge').text(parseInt(amount) + 1); break;
        case '-': $('#amountBasketItBadge').text(parseInt(amount) - 1); break;
    }
    if (currentNumber != -1) $('#amountBasketItBadge').text(currentNumber);
}
function OpenModelOrder(idOrder) {
    $.ajax({
        type: 'get',
        url: '/Manage/GetOrder',
        data: { 'id': idOrder },
        success: function (order) {
            $('#nameZak').text(order.user.name)
            $('#surnameZak').text(order.user.surname)
            $('#patronZak').text(order.user.patronymic)
            $('#phoneZak').text(order.user.phoneNumber)
            $('#nameProd').text(order.product.name)
            $('#catProd').text(order.product.category.name)
            $('#priceProd').text(order.product.price)
            $('#orderFinalPrice').text(order.finalPrice)
            $('#amountOrderProd').text(order.amountProduct)
            $('#dateOrder').text(order.date)
            $('#span-orderID').text(order.id)
            $('#message-box').hide()
            let modal = new bootstrap.Modal(document.querySelector('#exampleModal'));
            modal.show();
        },
        error: function (req, status, error) {
            console.log(req.status);
            console.log(req.responseText);
        }
    })
}
function CompleteOrder() {
    let prod = $('#span-orderID').text()
    $('#message-box').hide(100)
    $.ajax({
        type: 'post',
        url: '/Manage/CompleteOrder',
        data: { 'id': prod },
        success: function (msg) {
            $('#message-box').text('Успешно').addClass('bg-success').removeClass('bg-danger').show(500);
        },
        error: function (req, status, error) {
            $('#message-box').text(req.responseText).addClass('bg-danger').removeClass('bg-success').show(500);
            console.log(req.status);
            console.log(req.responseText);
        }
    })
}
function CancelOrder() {
    let prod = $('#span-orderID').text()
    $('#message-box').hide(100)
    $.ajax({
        type: 'post',
        url: '/Manage/CancelOrder',
        data: { 'id': prod },
        success: function (msg) {
            $('#message-box').text('Успешно').addClass('bg-success').removeClass('bg-danger').show(500);
        },
        error: function (req, status, error) {
            $('#message-box').text(req.responseText).addClass('bg-danger').removeClass('bg-success').show(500);
            console.log(req.status);
            console.log(req.responseText);
        }
    })
}
