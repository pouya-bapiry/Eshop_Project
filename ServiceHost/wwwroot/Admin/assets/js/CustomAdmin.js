const cookieWishlist = "wishlist-items";
let productColorPrice = 0;

function open_waiting(selector = 'body') {
    $(selector).waitMe({
        effect: 'bouncePulse',
        text: 'لطفا صبر کنید ...',
        bg: 'rgba(255,255,255,0.7)',
        color: '#000',
        fontSize: '18px',
    });
}

function close_waiting(selector = 'body') {
    $(selector).waitMe('hide');
}

function ShowMessage(title, text, theme) {
    window.createNotification({
        closeOnClick: true,
        displayCloseButton: false,
        positionClass: 'nfc-bottom-right',
        showDuration: 5000,
        theme: theme !== '' ? theme : 'success'
    })({
        title: title !== '' ? title : 'اعلان',
        message: decodeURI(text)
    });
}

$(document).ready(function () {
    var editors = $("[ckeditor]");
    if (editors.length > 0) {
        $.getScript('/Admin/assets/js/ckeditor.js', function () {
            $(editors).each(function (index, value) {
                var id = $(value).attr('ckeditor');
                ClassicEditor.create(document.querySelector('[ckeditor="' + id + '"]'),
                    {
                        toolbar: {
                            items: [
                                'heading',
                                '|',
                                'bold',
                                'italic',
                                'link',
                                '|',
                                'fontSize',
                                'fontColor',
                                '|',
                                'imageUpload',
                                'blockQuote',
                                'insertTable',
                                'undo',
                                'redo',
                                'codeBlock'
                            ]
                        },
                        language: 'fa',
                        table: {
                            contentToolbar: [
                                'tableColumn',
                                'tableRow',
                                'mergeTableCells'
                            ]
                        },
                        licenseKey: '',
                        simpleUpload: {
                            // The URL that the images are uploaded to.
                            uploadUrl: '/Uploader/UploadImage'
                        }

                    })
                    .then(editor => {
                        window.editor = editor;
                    }).catch(err => {
                        console.error(err);
                    });
            });
        });
    }


});

function FillPageId(pageId) {
    $('#PageId').val(pageId);
    $('#filter-form').submit();
}

$('[main_category_checkbox]').on('change', function (e) {
    var isChecked = $(this).is(':checked');
    var selectedCategoryId = $(this).attr('main_category_checkbox');
    console.log(selectedCategoryId);
    if (isChecked) {
        $('#sub_categories_' + selectedCategoryId).slideDown(300);
    } else {
        $('#sub_categories_' + selectedCategoryId).slideUp(300);
        $('[parent-category-id="' + selectedCategoryId + '"]').prop('checked', false);
    }
});

// Add an event listener to the product checkboxes
const productCheckboxes = document.querySelectorAll('input[name="SelectedProducts"]');
productCheckboxes.forEach(checkbox => {
    checkbox.addEventListener('change', function () {
        const productId = this.value;
        const productSizesContainer = document.querySelector(`#productSizes_${productId}`);
        if (this.checked) {
            productSizesContainer.style.display = 'block'; // Show the product sizes container
        } else {
            productSizesContainer.style.display = 'none'; // Hide the product sizes container
        }
    });
});





$('#add_color_button').on('click', function (e) {

    e.preventDefault();
    var colorName = $('#product_color_name_input').val();
    var colorPrice = $('#product_color_price_input').val();
    var colorCode = $('#product_color_code_input').val();

    if (colorName !== '' && colorPrice !== '' && colorCode !== '') {
        var currentColorsCount = $('#list_of_product_colors tr');
        var index = currentColorsCount.length;


        var isExistsSelectedColor = $('[color-name-hidden-input][value="' + colorName + '"]');

        if (isExistsSelectedColor.lenght !== 0) {

            var colorNameNode = `<input type="hidden" value="${colorName}" name="ProductColors[${index}].ColorName" color-name-hidden-input="${colorName}-${colorPrice}">`;
            var colorPriceNode = `<input type="hidden" value="${colorPrice}" name="ProductColors[${index}].Price" color-price-hidden-input="${colorName}-${colorPrice}">`;
            var colorCodeNode = `<input type="hidden" value="${colorCode}" name="ProductColors[${index}].ColorCode" color-code-hidden-input="${colorName}-${colorPrice}">`;

            $('#create_product_form').append(colorNameNode);
            $('#create_product_form').append(colorPriceNode);
            $('#create_product_form').append(colorCodeNode);


            var colorTableNode = `
          <tr color-table-item="${colorName}-${colorPrice}">
          <td>${colorName}</td> 
          <td>${colorPrice}</td>  
          <td>
          <div style="border-radius: 50%; width:40px; height: 40px; border:2px solid black; background-color: ${colorCode}"></div>
          </td>
          <td> <a class="btn btn-lg text-danger" style="float: none;" title="حذف رنگ" onclick="removeProductColor('${colorName}-${colorPrice}')">
          <i class="fa fa-trash"></i>
          </a>
          </td>
          </tr>`;
            $('#list_of_product_colors').append(colorTableNode);
            $('#product_color_name_input').val('');
            $('#product_color_price_input').val('');
            $('#product_color_code_input').val('');


        } else {
            ShowMessage('اخطار', 'رنگ وارد شده تکراری می باشد', 'warning');
            $('#product_color_name_input').val('');
            $('#product_color_price_input').val('');
            $('#product_color_code_input').val('');

            $('#product_color_name_input').val('').focus();
        }

    } else {
        ShowMessage('اخطار', 'لطفا نام رنگ و قیمت آن را به درستی وارد نمایید', 'warning');
    }

});


$('#add_feature_button').on('click', function (e) {

    e.preventDefault();
    var featureTitle = $('#product_feature_title_input').val();
    var featureValue = $('#product_feature_value_input').val();

    if (featureTitle !== '' && featureValue !== '') {

        var currentFeaturesCount = $('#list_of_product_features tr');
        var index = currentFeaturesCount.length;


        var isExistsSelectedFeature = $('[feature-title-hidden-input][value="' + featureTitle + '"]');

        if (isExistsSelectedFeature.lenght !== 0) {

            var featureTitleNode = `<input type="hidden" value="${featureTitle}" name="ProductFeatures[${index}].featureTitle" feature-title-hidden-input="${featureTitle}-${featureValue}">`;
            var featureValueNode = `<input type="hidden" value="${featureValue}" name="ProductFeatures[${index}].FeatureValue" feature-value-hidden-input="${featureTitle}-${featureValue}">`;

            $('#create_product_form').append(featureTitleNode);
            $('#create_product_form').append(featureValueNode);


            var featureTableNode = `
          <tr feature-table-item="${featureTitle}-${featureValue}">
          <td>${featureTitle}</td>
          <td>${featureValue}</td>
          <td> <a class="btn btn-lg text-danger" style="float: none;" title="حذف ویژگی" onclick="removeProductFeature('${featureTitle}-${featureValue}')">
          <i class="fa fa-trash"></i>
          </a>
          </td>
          </tr>`;
            $('#list_of_product_features').append(featureTableNode);
            $('#product_feature_title_input').val('');
            $('#product_feature_value_input').val('');

        } else {
            ShowMessage('اخطار', 'ویژگی وارد شده تکراری می باشد', 'warning');
            $('#product_feature_title_input').val('');
            $('#product_feature_value_input').val('');

            $('#product_feature_title_input').val('').focus();
        }

    } else {
        ShowMessage('اخطار', 'لطفا نام ویژگی و مقدار آن را به درستی وارد نمایید', 'warning');
    }

});


$('#add_size_button').on('click', function (e) {

    e.preventDefault();
    var sizeTitle = $('#product_size_title_input').val();
    var sizePrice = $('#product_size_price_input').val();

    if (sizeTitle !== '' && sizePrice !== '') {

        var currentSizeCount = $('#list_of_product_sizes tr');
        var index = currentSizeCount.length;


        var isExistsSelectedSize = $('[size-title-hidden-input][value="' + sizeTitle + '"]');

        if (isExistsSelectedSize.lenght !== 0) {

            var sizeTitleNode = `<input type="hidden" value="${sizeTitle}" name="ProductSizes[${index}].sizeTitle" size-title-hidden-input="${sizeTitle}-${sizePrice}">`;
            var sizePriceNode = `<input type="hidden" value="${sizePrice}" name="ProductSizes[${index}].sizePrice" size-price-hidden-input="${sizeTitle}-${sizePrice}">`;

            $('#create_product_form').append(sizeTitleNode);
            $('#create_product_form').append(sizePriceNode);


            var sizeTableNode = `
          <tr size-table-item="${sizeTitle}-${sizePrice}">
          <td>${sizeTitle}</td>
          <td>${sizePrice}</td>
          <td> <a class="btn btn-lg text-danger" style="float: none;" title="حذف سایز" onclick="removeProductSize('${sizeTitle}-${sizePrice}')">
          <i class="fa fa-trash"></i>
          </a>
          </td>
          </tr>`;
            $('#list_of_product_sizes').append(sizeTableNode);
            $('#product_size_title_input').val('');
            $('#product_size_price_input').val('');

        } else {
            ShowMessage('اخطار', 'سایز وارد شده تکراری می باشد', 'warning');
            $('#product_size_title_input').val('');
            $('#product_size_price_input').val('');

            $('#product_size_title_input').val('').focus();
        }

    } else {
        ShowMessage('اخطار', 'لطفا عنوان سایز  و قیمت آن را به درستی وارد نمایید', 'warning');
    }

});

function removeProductColor(index) {
    $('[color-name-hidden-input="' + index + '"]').remove();
    $('[color-price-hidden-input="' + index + '"]').remove();
    $('[color-code-hidden-input="' + index + '"]').remove();

    $('[color-table-item="' + index + '"]').remove();
    reOrderProductColorHiddenInputs();
}

function removeProductFeature(index) {
    $('[feature-title-hidden-input="' + index + '"]').remove();
    $('[feature-value-hidden-input="' + index + '"]').remove();

    $('[feature-table-item="' + index + '"]').remove();
    reOrderProductFeatureHiddenInputs();
}

function removeProductSize(index) {
    $('[size-title-hidden-input="' + index + '"]').remove();
    $('[size-price-hidden-input="' + index + '"]').remove();

    $('[size-table-item="' + index + '"]').remove();
    reOrderProductSizeHiddenInputs();
}

function reOrderProductColorHiddenInputs() {
    var hiddenColors = $('[color-name-hidden-input]');
    $.each(hiddenColors, function (index, value) {

        var hiddenColor = $(value);
        var colorId = $(value).attr('color-name-hidden-input');
        var hiddenPrice = $('[color-price-hidden-input="' + colorId + '"]');
        var hiddenCode = $('[color-code-hidden-input="' + colorId + '"]');

        $(hiddenColor).attr('name', 'ProductColors[' + index + '].ColorName');
        $(hiddenPrice).attr('name', 'ProductColors[' + index + '].Price');
        $(hiddenCode).attr('name', 'ProductColors[' + index + '].ColorCode');
    });
}

function reOrderProductFeatureHiddenInputs() {
    var hiddenFeatures = $('[feature-title-hidden-input]');
    $.each(hiddenFeatures, function (index, value) {

        var hiddenFeature = $(value);
        var featureId = $(value).attr('feature-title-hidden-input');
        var hiddenFeatureValue = $('[feature-value-hidden-input="' + featureId + '"]');

        $(hiddenFeature).attr('name', 'ProductFeatures[' + index + '].FeatureTitle');
        $(hiddenFeatureValue).attr('name', 'ProductFeatures[' + index + '].FeatureValue');
    });
}

function reOrderProductSizeHiddenInputs() {
    var hiddenSize = $('[size-title-hidden-input]');
    $.each(hiddenSize, function (index, value) {

        var hiddenSize = $(value);
        var sizeId = $(value).attr('size-title-hidden-input');
        var hiddenSizePrice = $('[size-price-hidden-input="' + sizeId + '"]');

        $(hiddenSize).attr('name', 'ProductSizes[' + index + '].SizeTitle');
        $(hiddenSizePrice).attr('name', 'ProductSizes[' + index + '].SizePrice');
    });
}



// Image Upload File
function showPreview(event, previewId) {
    if (event.target.files.length > 0) {
        var src = URL.createObjectURL(event.target.files[0]);
        var preview = document.getElementById("file-ip-" + previewId + "-preview");
        preview.src = src;
        preview.style.display = "block";
    }
}


const actualBtn = document.getElementById('actual-btn');

const fileChosen = document.getElementById('file-chosen');

actualBtn.addEventListener('change', function () {
    fileChosen.textContent = this.files[0].name
})
