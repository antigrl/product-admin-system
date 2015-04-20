$(function () {
    $.validator.unobtrusive.parseDynamicContent = function (selector) {
        //use the normal unobstrusive.parse method
        $.validator.unobtrusive.parse(selector);

        //get the relevant form
        var form = $(selector).first().closest('form');

        //get the collections of unobstrusive validators, and jquery validators
        //and compare the two
        var unobtrusiveValidation = form.data('unobtrusiveValidation');
        var validator = form.validate();

        $.each(unobtrusiveValidation.options.rules, function (elname, elrules) {
            if (validator.settings.rules[elname] == undefined) {
                var args = {};
                $.extend(args, elrules);
                args.messages = unobtrusiveValidation.options.messages[elname];
                //edit:use quoted strings for the name selector
                $("[name='" + elname + "']").rules("add", args);
            } else {
                $.each(elrules, function (rulename, data) {
                    if (validator.settings.rules[elname][rulename] == undefined) {
                        var args = {};
                        args[rulename] = data;
                        args.messages = unobtrusiveValidation.options.messages[elname][rulename];
                        //edit:use quoted strings for the name selector
                        $("[name='" + elname + "']").rules("add", args);
                    }
                });
            }
        });
    }

    $(".savePresentation").bind("click", function () {
        var onEventSavePresentation = new postPresentationSheet();
        onEventSavePresentation.launchMajorCategories();
        onEventSavePresentation.launchProducts();
    });

    function postPresentationSheet() {
        this.launchMajorCategories = function () {
            var majorCategories = [];
            var campaignID = $(".campaign-id").val();
            var count = 0;
            $(".major-category").each(function (index) {
                var id = $(this).find(".id").val();
                var categoryID = $(this).find(".category-id").val();
                var categoryRename = $(this).find(".major-title").val();
                var status = "Active";
                var showCategory = $(this).is(":visible");

                // build json object
                var category = {
                    ID: id,
                    SortValue: count,
                    CampaignID: campaignID,
                    CategoryID: categoryID,
                    CategoryRename: categoryRename,
                    Status: status,
                    ShowCategory: showCategory
                };
                majorCategories.push(category);
                count++;
            });

            var test = JSON.stringify(majorCategories);

            $.ajax({
                type: "POST",
                url: $(".save-major-category-url").val(),
                traditional: true,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: test,
                success: function (data) { console.log(data) },
                error: function (data) { console.log(data) }
            });
        }
        this.launchProducts = function () {
            var products = [];
            $(".major-category").each(function (index) {
                var count = 0;
                $(this).find(".product-row").each(function (index) {
                    var id = $(this).find(".product-id").val();

                    // build json object
                    var product = {
                        ID: id,
                        SortValue: count
                    };
                    products.push(product);
                    count++;
                });
            });

            var test = JSON.stringify(products);

            $.ajax({
                type: "POST",
                url: $(".save-product-url").val(),
                traditional: true,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: test,
                success: function (data) { console.log(data) },
                error: function (data) { console.log(data) }
            });
        }
    }

    var ajaxFormSubmit = function () {
        var $form = $(this);

        var options = {
            url: $form.attr("action"),
            type: $form.attr("method"),
            data: $form.serialize()
        };

        $.ajax(options).done(function (data) {
            var $target = $($form.attr("data-npr-target"));
            $target.replaceWith(data);
        });

        return false;
    };

    $("form[data-npr-ajax='true']").submit(ajaxFormSubmit);

    // Add Fee row to the page [currently only does one at a time
    $(".add-item").click(function () {
        $.ajax({
            type: "POST",
            url: this.href,
            cache: false,
            success: function (html) {
                $(this).parent().siblings(".add-target").append(html);

                $.validator.unobtrusive.parseDynamicContent('form');
            },
            context: this
        });
        return false;
    });

    //Removes Dollar Fees
    $("body").on("click", ".delete-row", function () {
        var $feeRow = $(this).closest(".editor-row");
        var companyFee;

        // Check if inherited:
        var isInherited = $feeRow.find(".is-inherited input").val();
        if (isInherited == "True") {
            // Editor Row Data
            var inheritedFeeID = $feeRow.find(".inherited-id input").val();

            $(".existing-fee-checkbox").each(function () {
                // Check the Company Fee Values for the connected product
                var companyFeeHref = $(this).siblings(".add-existing-fee").attr("href").split("&");
                var companyFeeID = decodeURI(companyFeeHref[2].split("=")[1]);

                if (inheritedFeeID == companyFeeID) {
                    companyFee = $(this);
                }
            });
        }

        if (confirm("Are you sure you want to remove this?")) {
            // Remove the fee and uncheck the company fee if it was inherited
            $feeRow.remove();
            companyFee.attr("checked", false);

            return false;
        }
        else {
            // Recheck the box if they decide to cancel
            companyFee.attr("checked", true);
        }
    });

    // Add Existing Fee Checkbox
    $(".existing-fee-checkbox").change(function () {
        // TODO: add section for campaign fee
        var companyFeeHref = $(this).siblings(".add-existing-fee").attr("href").split("&");
        var companyFeeType = decodeURI(companyFeeHref[1].split("=")[1]);
        var companyFeeID = decodeURI(companyFeeHref[2].split("=")[1]);

        if ($(this).prop("checked")) {
            // Add the Fee
            $.ajax({
                type: "POST",
                url: $(this).siblings(".add-existing-fee").attr("href"),
                cache: false,
                success: function (html) {
                    $($(this).attr("data-html-location")).append(html);

                    // Disable all options other than what was selected
                    $($(this).attr("data-html-location") + " .editor-row:last select option:not(:selected)").each(function () {
                        $(this).prop("disabled", "disabled");
                    });

                    $($(this).attr("data-html-location") + " .editor-row:last input:radio:not(:checked)").each(function () {
                        $(this).prop("disabled", "disabled");
                    });
                },
                context: this
            });
            return false;
        }
        else {
            // Remove the Fee
            $(".editor-row").each(function () {
                // if it's inherited
                var isInherited = $(this).find(".is-inherited input").val();
                if (isInherited == "True") {
                    // Tie Existing fee checkbox to the inherited fee
                    var inheritedFeeID = $(this).find(".inherited-id input").val();

                    if (companyFeeID == inheritedFeeID) {
                        $(this).find(".delete-row").click();
                    }
                }
            });
        }
    });

    $("#FeeType").change(function () {
        ShowFeeInputBasedOnType();
    });

    // $(".toggle-company-fees").click(function () {
    //     $(".company-fees").toggle();
    // });

    $(".toggle-archived-products").click(function () {
        $(".archived-products").toggle();
    });

    $(".showPrintPreview ").click(function () {
        $(".presentation-sheet-area").toggle();
        $(".drag-and-drop-area").toggle();
    });

    // Presentation view functions
    $(".pricing-tier-checkbox").change(function () {
        var value = $(this).val();
        $("." + value).toggle();
    });

    $(".major-catgory-checkbox").change(function () {
        //Hide the category
        var value = $(this).val();
        $("." + value).toggle();

        //update page numbers
        ShowPrintNumbers();
    });

    $(".starting-page-number").change(function () {
        ShowPrintNumbers();
    });

    ShowPrintNumbers();
    // End Presentation view functions

    $("form").submit(function () {
        if ($(this).valid()) {
            $(this).find("input[type='submit']").attr('disabled', true);
            return true;
        }
    });

    DisabledInheritedFees();
    HideDropdownsForForeignKeyValues();
    ShowFeeInputBasedOnType();

    function ShowPrintNumbers() {
        var startingPageNumber = $(".starting-page-number").val();
        $(".page_number").each(function () {
            if ($(this).is(":visible") == true) {
                $(this).find("span").text("Page " + startingPageNumber);
                startingPageNumber++;
            }
        });
    }

    function ShowFeeInputBasedOnType() {
        // Hide all options [for Fee Create Page]
        $(".dollar-amount-fee").hide();
        $(".amortized-fee").hide();
        $(".percent-fee").hide();

        var feeType = $("#FeeType option:selected").text();
        // Set text to lower rand replace a space with a dash
        feeType = feeType.toLowerCase().replace(" ", "-");
        $("." + feeType + "-fee").show();
    }

    function HideDropdownsForForeignKeyValues() {
        // For if Company Is selected
        if ($("#CompanyID option:selected").val() > 0) {
            $("#CompanyID option:not(:selected)").each(function () {
                $(this).prop("disabled", "disabled");
            });

            $(".company-data").show();
            $(".campaign-data").hide();
            $(".pricingtier-data").hide();
            $(".product-data").hide();
            $(".productsellprice-data").hide();
        }
            // For if campaign is selected
        else if ($("#CampaignID option:selected").val() > 0) {
            $("#CampaignID option:not(:selected)").each(function () {
                $(this).prop("disabled", "disabled");
            });

            $(".company-data").hide();
            $(".campaign-data").show();
            $(".pricingtier-data").hide();
            $(".product-data").hide();
            $(".productsellprice-data").hide();
        }
            // For if pricingTier is selected
        else if ($("#PricingTierID option:selected").val() > 0) {
            $("#PricingTierID option:not(:selected)").each(function () {
                $(this).prop("disabled", "disabled");
            });

            $(".company-data").hide();
            $(".campaign-data").hide();
            $(".pricingtier-data").show();
            $(".product-data").hide();
            $(".productsellprice-data").hide();
        }
            // For if prouct is selected
        else if ($("#ProductID option:selected").val() > 0) {
            $("#ProductID option:not(:selected)").each(function () {
                $(this).prop("disabled", "disabled");
            });

            $(".company-data").hide();
            $(".campaign-data").hide();
            $(".pricingtier-data").hide();
            $(".product-data").show();
            $(".productsellprice-data").hide();
        }
            // For if prouctSellPrice is selected
        else if ($("#ProductSellPriceID option:selected").val() > 0) {
            $("#ProductSellPriceID option:not(:selected)").each(function () {
                $(this).prop("disabled", "disabled");
            });

            $(".company-data").hide();
            $(".campaign-data").hide();
            $(".pricingtier-data").hide();
            $(".product-data").hide();
            $(".productsellprice-data").show();
        }
    }

    function DisabledInheritedFees() {
        // Loop though all fee rows
        $(".fees .editor-row").each(function () {
            // Check if the fee is inherited
            var isInherited = $(this).find(".is-inherited input").val();
            if (isInherited == "True") {
                // Add Inherited Class
                $(this).addClass("inherited");

                // Disable all not selected options if inherited
                $(this).find("select option:not(:selected)").each(function () {
                    $(this).prop("disabled", "disabled");
                });

                // Disable all radio buttons if inherited [if applicable]
                $(this).find("input:radio:not(:checked)").each(function () {
                    $(this).prop("disabled", "disabled");
                });

                // Tie Existing fee checkbox to the inherited fee
                var inheritedFeeID = $(this).find(".inherited-id input").val();

                // TODO: add section for campaign fees
                $(".company-fees .company-fee").each(function () {
                    var companyFeeHref = $(this).find(".add-existing-fee").attr("href").split("&");
                    var companyFeeID = decodeURI(companyFeeHref[2].split("=")[1]);

                    // TODO: Check each aspect of the fee [name, type, calculation]
                    if (inheritedFeeID == companyFeeID) {
                        $(this).find(".existing-fee-checkbox").prop("checked", true);
                    }
                });
            }
        });
    }
});
$('.description-container').html(function (i, t) {
    return t.replace('[eco]', '<span class="hidden">[eco]</span>');
});
$('.description-container').html(function (i, t) {
    return t.replace('[recycle]', '<span class="hidden">[recycle]</span>');
});
$('.description-container').html(function (i, t) {
    return t.replace('[dish]', '<span class="hidden">[dish]</span>');
});
$('.description-container').html(function (i, t) {
    return t.replace('[usa]', '<span class="hidden">[usa]</span>');
});

$('table td div.description-container:contains("[eco]")').each(function () {
    $(this).parent().parent().parent().parent().prev().find('.icon-container .eco').show();
});

$('table td div.description-container:contains("[dish]")').each(function () {
    $(this).parent().parent().parent().parent().prev().find('.icon-container .dish').show();
});

$('table td div.description-container:contains("[recycle]")').each(function () {
    $(this).parent().parent().parent().parent().prev().find('.icon-container .rec').show();
});

$('table td div.description-container:contains("[usa]")').each(function () {
    $(this).parent().parent().parent().parent().prev().find('.icon-container .usa').show();
});