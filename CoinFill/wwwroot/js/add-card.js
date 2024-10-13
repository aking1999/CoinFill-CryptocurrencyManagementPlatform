$(document).ready(function () {
    var selectedCardType;
    var selectedCardBrand;

    const cryptoPaymentMethods = $('#Chosen_PaymentMethodId');
    const cryptoNetworks = $('#network');
    const cryptoNetworksDropdownHolder = $('#network-dropdown-holder');

    const qrAddressHolder = $('#qr-address-holder');
    const qrAddress = $('#qr-address');
    const notice = $('#notice');
    const address = $('#address');
    const copy = $('#copy');

    const alertBody = $('#alert-body');
    const alertMessage = $('#alert-message');

    const btnFinishPayment = $('#btn-finish-payment');
    const btnFinishPaymentText = btnFinishPayment.find('span');
    const btnFinishPaymentTextUnchaned = btnFinishPaymentText.html();

    let cardTypeModal = $('#card-type-modal');
    let cardBrandModal = $('#card-brand-modal');
    let paymentMethodsModal = $('#payment-methods-modal');

    const csfrToken = $("input[name='__RequestVerificationToken']");
    const creditCardForm = $('#create-credit-card-form');

    $('button[data-card-type]').click(function (event) {
        event.preventDefault();
        DisableInputs();
        let targetButton = $(this);
        let targetButtonText = targetButton.find('span');
        let targetButtonTextUnchanged = targetButtonText.html();
        selectedCardType = targetButton.attr('data-card-type');
        $('#card-type-holder').text(selectedCardType);

        if (targetButton.attr('data-card-type') === 'physical') {
            let currentAddress = targetButton.attr('data-address');
            EnableInputs();
            if (typeof currentAddress !== 'undefined' && currentAddress !== false && currentAddress) {
                Swal.mixin({
                    customClass: {
                        confirmButton: "btn btn-sm btn-primary mr-2",
                        cancelButton: "btn btn-sm btn-falcon-secondary"
                    },
                    buttonsStyling: false
                }).fire({
                    title: 'Is your shipping address correct?',
                    html: `Shipping address for the physical card:<br>${currentAddress}`,
                    icon: 'question',
                    showCancelButton: true,
                    confirmButtonText: "Yes, it's correct",
                    cancelButtonText: "No, edit it"
                }).then((result) => {
                    if (result.isConfirmed) {
                        DisableInputs();

                        let height = targetButton.height();
                        let width = targetButton.width();
                        targetButtonText.fadeOut('fast', function () {
                            targetButtonText.html('<div class="spinner mx-auto"></div>').fadeIn('slow');
                            targetButton.height(height);
                            targetButton.width(width);
                        })

                        setTimeout(function () {
                            targetButtonText.fadeOut('fast', function () {
                                $(this).html('<i class="ca-lightgreen fas fa-check"></i>').fadeIn(function () {
                                    setTimeout(function () {
                                        cardTypeModal.modal('hide');
                                        cardBrandModal.modal();
                                        EnableInputs();
                                        targetButtonText.fadeOut('fast', function () {
                                            $(this).html(targetButtonTextUnchanged).fadeIn(100);
                                        })
                                    }, 1000);
                                });
                            })
                        }, 1200)
                    } else {
                        window.location.href = addAddressUrl;
                    }
                });
            } else {
                Swal.fire({
                    title: "Address required for shipment of physical Crypto card",
                    text: "Please add the address to which you would like the physical Crypto card to be shipped.",
                    icon: "info",
                    showCancelButton: false,
                    confirmButtonColor: "#27bcfd",
                    confirmButtonText: "Add shipping address"
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.href = addAddressUrl;
                    }
                });
            }
        } else {
            let height = targetButton.height();
            let width = targetButton.width();
            targetButtonText.fadeOut('fast', function () {
                //targetButtonText.html('<div class="spinner mx-auto" style="border:1px solid var(--primary) !important;border-top-color: white !important;"></div>').fadeIn('slow');
                targetButtonText.html('<div class="spinner mx-auto"></div>').fadeIn('slow');
                targetButton.height(height);
                targetButton.width(width);
            })

            setTimeout(function () {
                targetButtonText.fadeOut('fast', function () {
                    $(this).html('<i class="ca-lightgreen fas fa-check"></i>').fadeIn(function () {
                        setTimeout(function () {
                            cardTypeModal.modal('hide');
                            cardBrandModal.modal();
                            EnableInputs();
                            targetButtonText.fadeOut('fast', function () {
                                $(this).html(targetButtonTextUnchanged).fadeIn(100);
                            })
                        }, 1000);
                    });
                })
            }, 1200)
        }
    });

    $('button[data-card-brand]').click(function (event) {
        event.preventDefault();
        DisableInputs();
        let targetButton = $(this);
        let targetButtonText = targetButton.find('span');
        let targetButtonTextUnchanged = targetButtonText.html();
        selectedCardBrand = targetButton.attr('data-card-brand');
        $('#card-brand-holder').text(selectedCardBrand);

        let height = targetButton.height();
        targetButtonText.fadeOut('fast', function () {
            targetButtonText.html('<div class="spinner mx-auto" style="border:1px solid var(--primary) !important;border-top-color: white !important;"></div>').fadeIn('slow');
            targetButton.height(height);
        })

        setTimeout(function () {
            targetButtonText.fadeOut('fast', function () {
                $(this).html('<i class="text-success fas fa-check"></i>').fadeIn(function () {
                    setTimeout(function () {
                        cardTypeModal.modal('hide');
                        cardBrandModal.modal('hide');
                        paymentMethodsModal.modal('show');
                        EnableInputs();
                        targetButtonText.fadeOut('fast', function () {
                            $(this).html(targetButtonTextUnchanged).fadeIn(100);
                        })
                    }, 1000);
                })
            })
        }, 1200)
    })

    cryptoPaymentMethods.change(function () {
        cryptoNetworks.selectpicker('val', '');
        cryptoNetworks.selectpicker('refresh');
        cryptoNetworksDropdownHolder.css('display', 'none');
        qrAddressHolder.removeClass('row').css('display', 'none');
        alertBody.removeClass('d-flex').css('display', 'none');
        $('#payment-modal-footer').removeClass('pt-3').addClass('pt-0');
        btnFinishPayment.css('display', 'none');
        $('#network option').remove();

        $.ajax({
            url: getNetworks,
            type: 'POST',
            data: { coinId: cryptoPaymentMethods.find(":selected").val() },
            dataType: 'json',
            headers: {
                'RequestVerificationToken': csfrToken.val()
            },
            success: function (response) {
                if (response.success === true) {
                    $.each(response.networks, function (i, obj) {
                        var ul = document.getElementById("network");
                        var li = document.createElement("option");
                        var linkText = document.createTextNode(obj.name);
                        li.value = obj.id;
                        li.appendChild(linkText);
                        ul.appendChild(li);
                    })

                    cryptoNetworks.selectpicker('refresh');
                    cryptoNetworksDropdownHolder.fadeIn('slow');

                    cryptoNetworks.change(function () {
                        qrAddressHolder.removeClass('row').css('display', 'none');
                        alertBody.removeClass('d-flex').css('display', 'none');
                        $('#payment-modal-footer').removeClass('pt-3').addClass('pt-0');
                        btnFinishPayment.css('display', 'none');
                        qrAddress.css(
                            {
                                'background-image': `none`,
                                'background-size': 'none'
                            }).addClass('avatar avatar-4xl').html('')

                        $.ajax({
                            url: getUserQrCode,
                            type: 'POST',
                            data: {
                                coinId: cryptoPaymentMethods.find(":selected").val(),
                                networkId: cryptoNetworks.find(":selected").val(),
                                cardType: selectedCardType,
                                cardBrand: selectedCardBrand
                            },
                            dataType: 'json',
                            headers: {
                                'RequestVerificationToken': csfrToken.val()
                            },
                            success: function (response) {
                                if (response.success === true) {
                                    if (response.generated === true) {
                                        qrAddress.css(
                                            {
                                                'background-image': `url('/generated/deposit/addresses/${response.qrCode}')`,
                                                'background-size': '100% 100%'
                                            })

                                        notice.html(response.notice);
                                        address.val(response.address);

                                        notice.fadeIn(1);
                                        address.fadeIn(1);
                                        copy.fadeIn(1);

                                        qrAddressHolder.fadeIn(1, function () {
                                            $(this).addClass('row');
                                        });

                                        alertBody.fadeIn(1, function () {
                                            $(this).addClass('d-flex');
                                            alertMessage.fadeIn('slow');
                                        });

                                        $('#payment-modal-footer').removeClass('pt-0').addClass('pt-3');
                                        btnFinishPayment.fadeIn('slow');
                                    } else {
                                        qrAddress
                                            .removeClass('avatar avatar-4xl')
                                            .html(
                                                `<div id="address-not-generated" class="mb-3 text-center">You have not generated a ${$('#Chosen_PaymentMethodId option:selected').text().trim()} (${$('#network option:selected').text().trim()}) address yet. Click the button below to generate a new ${$('#Chosen_PaymentMethodId option:selected').text().trim()} (${$('#network option:selected').text().trim()}) address.</div>` +
                                                '<span class="d-flex justify-content-center">' +
                                                '<button id="btn-generate" class="btn btn-primary btn-sm">' +
                                                '<span id="btn-generate-text">Generate new address</span>' +
                                                '</button>' +
                                                '</span>')

                                        notice.fadeOut(1);
                                        address.fadeOut(1);
                                        copy.fadeOut(1);

                                        qrAddressHolder.fadeIn('slow', function () {
                                            $(this).addClass('row');

                                            let btnGenerate = $('#btn-generate');
                                            let btnGenerateText = btnGenerate.find('#btn-generate-text');
                                            const btnGenerateTextUnchanged = btnGenerateText.html();

                                            btnGenerate.click(function (event) {
                                                event.preventDefault();
                                                event.stopImmediatePropagation();
                                                btnGenerate.prop('disabled', true);
                                                DisableInputs();

                                                let width = btnGenerate.width();
                                                let height = btnGenerate.height();
                                                btnGenerateText.fadeOut('fast', function () {
                                                    btnGenerateText.html('<div class="spinner mx-auto"></div>').fadeIn('slow');
                                                    btnGenerate.height(height);
                                                    btnGenerate.width(width);
                                                })

                                                setTimeout(function () {
                                                    $.ajax({
                                                        url: generateNetworkAddress,
                                                        type: 'POST',
                                                        data: {
                                                            coinId: cryptoPaymentMethods.find(":selected").val(),
                                                            networkId: cryptoNetworks.find(":selected").val(),
                                                            cardType: selectedCardType,
                                                            cardBrand: selectedCardBrand
                                                        },
                                                        dataType: 'json',
                                                        headers: {
                                                            'RequestVerificationToken': csfrToken.val()
                                                        },
                                                        success: function (response) {
                                                            if (response.success === true) {
                                                                btnGenerateText.fadeOut('fast', function () {
                                                                    $(this).html('<i class="ca-lightgreen fas fa-check no-tick"></i>').fadeIn('slow', function () {
                                                                        setTimeout(function () {
                                                                            toastr[response.severity](response.body, response.title);
                                                                            qrAddress.fadeOut('fast', function () {
                                                                                $(this).html('');
                                                                                $(this).addClass('avatar avatar-4xl').css(
                                                                                    {
                                                                                        'background-image': `url('/generated/deposit/addresses/${response.qrCode}')`,
                                                                                        'background-size': '100% 100%'
                                                                                    });
                                                                                qrAddressHolder.fadeIn('slow', function () {
                                                                                    $(this).addClass('row');
                                                                                    qrAddress.fadeIn('slow');
                                                                                })

                                                                                notice.html(response.notice);
                                                                                address.val(response.address);

                                                                                notice.fadeIn('slow');
                                                                                address.fadeIn('slow');
                                                                                copy.fadeIn('slow');
                                                                                alertBody.fadeIn(1, function () {
                                                                                    $(this).addClass('d-flex');
                                                                                    alertMessage.fadeIn('slow');
                                                                                });
                                                                                btnFinishPayment.fadeIn('slow');
                                                                                EnableInputs();
                                                                            })
                                                                        }, 600);
                                                                    });
                                                                });
                                                            } else {
                                                                if (response.redirectUrl) window.location.href = response.redirectUrl;
                                                                else {
                                                                    btnGenerateText.fadeOut('fast', function () {
                                                                        $(this).html('<i class="text-danger fas fa-times no-tick"></i>').fadeIn('slow');
                                                                        HandleErrorShowSwal(response.title, response.body, response.severity);
                                                                    })
                                                                        .promise()
                                                                        .done(function () {
                                                                            setTimeout(function () {
                                                                                btnGenerateText.fadeOut('fast', function () {
                                                                                    $(this).html(btnGenerateTextUnchanged).fadeIn('slow');
                                                                                    EnableInputs();
                                                                                });
                                                                            }, 1300)
                                                                        })
                                                                }
                                                            }
                                                        },
                                                        error: function () {
                                                            HandleErrorShowSwal('An error occurred', 'Please refresh the page and try again or contact the customer support.', 'error');
                                                        }
                                                    })
                                                }, 1200)
                                            })
                                        })
                                    }
                                } else {
                                    if (response.redirectUrl) window.location.href = response.redirectUrl;
                                    else HandleErrorShowSwal(response.title, response.body, response.severity);
                                }
                            },
                            error: function () {
                                HandleErrorShowSwal('An error occurred', 'Please refresh the page and try again or contact the customer support.', 'error');
                            }
                        });
                    });
                } else {
                    if (response.redirectUrl) window.location.href = response.redirectUrl;
                    else if (response.networks && response.networks.length === 0) {
                        var ul = document.getElementById("network");
                        var li = document.createElement("option");
                        var linkText = document.createTextNode(`${$('#Chosen_PaymentMethodId option:selected').text().trim()} blockchain unavailable.`);
                        li.disabled = true;
                        li.appendChild(linkText);
                        ul.appendChild(li);

                        cryptoNetworks.selectpicker('refresh');
                        cryptoNetworksDropdownHolder.fadeIn('slow');
                    }
                    else HandleErrorShowSwal(response.title, response.body, response.severity);
                }
            },
            error: function () {
                HandleErrorShowSwal('An error occurred', 'Please refresh the page and try again or contact the customer support.', 'error');
            }
        });
    })

    btnFinishPayment.click(function (event) {
        event.preventDefault();
        event.stopImmediatePropagation();

        DisableInputs();

        let width = btnFinishPayment.width();
        let height = btnFinishPayment.height();
        btnFinishPaymentText.fadeOut('fast', function () {
            btnFinishPaymentText.html('<div class="spinner mx-auto"></div>').fadeIn('slow');
            btnFinishPayment.height(height);
            btnFinishPayment.width(width);
        });

        setTimeout(function () {
            $.ajax({
                url: getSenderAddressUrl,
                type: 'POST',
                data: {
                    coinId: cryptoPaymentMethods.find(":selected").val(),
                    networkId: cryptoNetworks.find(":selected").val(),
                    cardType: selectedCardType,
                    cardBrand: selectedCardBrand
                },
                dataType: 'json',
                headers: {
                    'RequestVerificationToken': csfrToken.val()
                },
                success: function (response) {
                    if (response.success === true) {
                        btnFinishPaymentText.fadeOut('fast', function () {
                            $(this).html('<i class="ca-lightgreen fas fa-check no-tick"></i>').fadeIn('slow', function () {
                                setTimeout(function () {
                                    paymentMethodsModal.modal('hide');
                                    cardBrandModal.modal('hide');
                                    cardTypeModal.modal('hide');
                                    btnFinishPaymentText.html(btnFinishPaymentTextUnchaned);
                                    $('html').append(response.partialView);
                                    RefreshPaymentInputs();
                                    EnableInputs();

                                    let activationStatusElement;
                                    if (response.card.activationStatus === 1) {
                                        activationStatusElement = '<span class="badge badge-soft-success">' +
                                            '<i class="fas fa-check-circle mr-1"></i>Active' +
                                            '</span>';
                                    } else if (response.card.activationStatus === 0) {
                                        activationStatusElement = '<span class="badge badge-soft-warning">' +
                                            '<i class="fas fa-stream mr-1"></i>Pending' +
                                            '</span>';
                                    } else if (response.card.activationStatus === -1) {
                                        activationStatusElement = `<button data-card-id="${response.card.id}" class="btn btn-danger btn-mini p-2 btn-confirm-card" type="button">` +
                                            '<span>Confirm wallet</span>' +
                                            '</button>';
                                    }

                                    let cardHolder;
                                    if ((response.card.firstName + ' ' + response.card.lastName).length > 14) {
                                        cardHolder = (response.card.firstName + ' ' + response.card.lastName).substring(0, 11) + '...'
                                    } else {
                                        cardHolder = response.card.firstName + ' ' + response.card.lastName
                                    }

                                    let newCardBody = '<div class="d-flex justify-content-center col-12 col-sm-6 col-md-6 col-lg-4 col-xl-3 text-center mb-3">' +
                                        `<div class="card card-span" style="background-image: url('/images/static/card-backgrounds/${response.card.image}'); width: 18rem; min-width: 16rem; height: 11rem; border-radius: 0.75rem; background-size: 100% 100% !important;">` +
                                        '<div class="card-body">' +
                                        `<div class="d-flex ${response.card.bootstrapClasses}">` +
                                        `<img src="/images/static/card-types-images/${response.card.brand}" width="${response.card.brandWidth}" height="${response.card.brandHeight}" />` +
                                        '<div class="justify-content-end ml-auto">' +
                                        activationStatusElement +
                                        '</div>' +
                                        '</div>' +
                                        '<div class="row">' +
                                        '<div class="col-12 text-black fs-2 font-weight-semi-bold text-left mt-3 mb-2">' +
                                        `$ ${response.card.balance}` +
                                        '</div>' +
                                        '<div class="col-12 text-black text-left font-weight-medium">' +
                                        `${response.card.number}` +
                                        '</div>' +
                                        '<div class="col-12 text-black d-flex justify-content-between">' +
                                        `<span data-toggle="tooltip" data-placement="bottom" data-html="true" title="<i class='fas fa-user text-info mr-1'></i>${response.card.firstName} ${response.card.lastName}" class="font-weight-semi-bold">` +
                                        `${cardHolder}` +
                                        '</span>' +
                                        '<span class="font-weight-medium">' +
                                        `${response.card.expiration}` +
                                        '</span>' +
                                        '<span class="font-weight-medium">' +
                                        `${response.card.cvv}` +
                                        '</span>' +
                                        '</div>' +
                                        '</div>' +
                                        '</div>' +
                                        '</div>' +
                                        '</div>';

                                    const addCardCol = $('#add-card-col');
                                    addCardCol.removeClass();
                                    addCardCol.addClass('d-flex justify-content-center col-12 col-sm-6 col-md-6 col-lg-4 col-xl-3');

                                    $(newCardBody).insertBefore('#add-card-col')
                                    $('[data-toggle="tooltip"]').tooltip();

                                    $(`button[data-card-id="${response.card.id}"]`).click(function () {
                                        FuncConfirmPayment($(this));
                                    })
                                }, 600)
                            })
                        })
                    } else {
                        if (response.redirectUrl) window.location.href = response.redirectUrl;
                        else if (response.addAddressUrl) {
                            btnFinishPaymentText.fadeOut('fast', function () {
                                $(this).html('<i class="text-danger fas fa-times no-tick"></i>').fadeIn('slow');
                                EnableInputs();
                                Swal.mixin({
                                    customClass: {
                                        confirmButton: "btn btn-sm btn-info mr-2",
                                        cancelButton: "btn btn-sm btn-falcon-secondary"
                                    },
                                    buttonsStyling: false
                                }).fire({
                                    title: response.title,
                                    text: response.body,
                                    icon: response.severity,
                                    showCancelButton: true,
                                    confirmButtonText: "Add shipping address",
                                    cancelButtonText: "Later"
                                }).then((result) => {
                                    if (result.isConfirmed) {
                                        window.location.href = response.orderCryptoCardUrl;
                                    }
                                });
                            })
                                .promise()
                                .done(function () {
                                    setTimeout(function () {
                                        btnFinishPaymentText.fadeOut('fast', function () {
                                            $(this).html(btnFinishPaymentTextUnchaned).fadeIn('slow');
                                            $('button[data-card-id]').prop('disabled', false);
                                        });
                                    }, 1300)
                                })
                        }
                        else {
                            btnFinishPaymentText.fadeOut('fast', function () {
                                $(this).html('<i class="text-danger fas fa-times no-tick"></i>').fadeIn('slow');
                                HandleErrorShowSwal(response.title, response.body, response.severity);
                            })
                                .promise()
                                .done(function () {
                                    setTimeout(function () {
                                        btnFinishPaymentText.fadeOut('fast', function () {
                                            $(this).html(btnFinishPaymentTextUnchaned).fadeIn('slow');
                                            $('button[data-card-id]').prop('disabled', false);
                                        });
                                    }, 1300)
                                })
                        }
                    }
                },
                error: function () {
                    btnFinishPaymentText.fadeOut('fast', function () {
                        $(this).html('<i class="text-danger fas fa-times no-tick"></i>').fadeIn('slow');
                        HandleErrorShowSwal('An error occurred', 'Please refresh the page and try again or contact the customer support.', 'error');
                    })
                        .promise()
                        .done(function () {
                            setTimeout(function () {
                                btnFinishPaymentText.fadeOut('fast', function () {
                                    $(this).html(btnFinishPaymentTextUnchaned).fadeIn('slow');
                                    $('button[data-card-id]').prop('disabled', false);
                                });
                            }, 1300)
                        })
                }
            })
        }, 1200)
    })

    $('button[data-card-id]').click(function () {
        FuncConfirmPayment($(this));
    })

    function FuncConfirmPayment(cardElement) {
        let node = $(cardElement);
        let cardElementText = node.find('span');
        let cardElementTextUnchanged = cardElementText.html();
        const cardId = node.attr('data-card-id');

        $('button[data-card-id]').prop('disabled', true);

        const width = node.width();
        const height = node.height();
        cardElementText.fadeOut('fast', function () {
            cardElementText.html('<div class="spinner mx-auto position-absolute" style="right: 22%; top: 14.5%; width: 1rem !important; height: 1rem !important; border-top-color: var(--danger) !important;"></div>').fadeIn('slow');
            node.width(width);
            node.height(height);
        });

        setTimeout(function () {
            $.ajax({
                url: confirmPaymentPartialUrl,
                type: 'POST',
                data: {
                    cardId: cardId
                },
                dataType: 'json',
                headers: {
                    'RequestVerificationToken': csfrToken.val()
                },
                success: function (response) {
                    if (response.success === true) {
                        $('html').append(response.partialView);
                    } else {
                        if (response.redirectUrl) window.location.href = response.redirectUrl;
                        else {
                            cardElementText.fadeOut('fast', function () {
                                cardElementText.html('<i class="text-white fas fa-times no-tick"></i>').fadeIn('slow');
                                HandleErrorShowSwal(response.title, response.body, response.severity);
                            })
                                .promise()
                                .done(function () {
                                    setTimeout(function () {
                                        cardElementText.fadeOut('fast', function () {
                                            $(this).html(cardElementTextUnchanged).fadeIn('slow');
                                            $('button[data-card-id]').prop('disabled', false);
                                        });
                                    }, 1300)
                                })
                        }
                    }
                },
                error: function () {
                    cardElementText.fadeOut('fast', function () {
                        $(this).html('<i class="fas fa-times text-white no-tick"></i>').fadeIn('slow');
                        HandleErrorShowSwal('An error occurred', 'Please refresh the page and try again or contact the customer support.', 'error');
                    })
                        .promise()
                        .done(function () {
                            setTimeout(function () {
                                cardElementText.fadeOut('fast', function () {
                                    $(this).html(cardElementTextUnchanged).fadeIn('slow');
                                    $('button[data-card-id]').prop('disabled', false);
                                });
                            }, 1300)
                        })
                }
            })
        }, 1200)
    }

    $('#btn-cancel-card-brand').click(function () {
        cardBrandModal.modal('hide');
        paymentMethodsModal.modal('hide');
        RefreshPaymentInputs();
        EnableInputs();
        cardTypeModal.modal('show');
    });

    $('#btn-cancel-payment').click(function () {
        paymentMethodsModal.modal('hide');
        RefreshPaymentInputs();
        cardTypeModal.modal('hide');
        EnableInputs();
        cardBrandModal.modal('show');
    })

    $('#card-type-times, #card-brand-times, #payment-methods-times').click(function () {
        RefreshPaymentInputs();
    })

    function RefreshPaymentInputs() {
        $('.selectpicker').selectpicker('val', '');
        $('.selectpicker').selectpicker('refresh');
        cryptoNetworksDropdownHolder.css('display', 'none');
        qrAddressHolder.removeClass('row').css('display', 'none');
        alertBody.removeClass('d-flex').css('display', 'none');
        $('#payment-modal-footer').removeClass('pt-3').addClass('pt-0');
        btnFinishPayment.css('display', 'none');
        $('#network option').remove();
    }

    function EnableInputs() {
        $('button[data-card-type]').prop('disabled', false);
        $('button[data-card-brand]').prop('disabled', false);

        $('#btn-cancel-card-brand').prop('disabled', false);

        btnFinishPayment.prop('disabled', false);
        $('#btn-cancel-payment').prop('disabled', false);

        $('#card-type-times').prop('disabled', false);
        $('#card-brand-times').prop('disabled', false);
        $('#payment-methods-times').prop('disabled', false);
    }

    function DisableInputs() {
        $('button[data-card-type]').prop('disabled', true);
        $('button[data-card-brand]').prop('disabled', true);

        $('#btn-cancel-card-brand').prop('disabled', true);

        btnFinishPayment.prop('disabled', true);
        $('#btn-cancel-payment').prop('disabled', true);

        $('#card-type-times').prop('disabled', true);
        $('#card-brand-times').prop('disabled', true);
        $('#payment-methods-times').prop('disabled', true);
    }

    function HandleErrorShowSwal(title, body, severity) {
        Swal.fire({
            icon: severity,
            title: title,
            text: body,
            footer: `<a href="${supportUrl}">Contact the customer support.</a>`
        });
    }
})