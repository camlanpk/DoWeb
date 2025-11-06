function InitLoadDataSelect2Ajax($el) {
    $el.find('select').each(function () {
        //  debugger
        let $this = $(this);

        let loadDanhSach = $this.data('execfunction');
        let dataLink = $this.data('linkdata');

        if (!loadDanhSach) return;

        ExecuteFunctionByName(loadDanhSach, window, $this, dataLink, null, null);
    });
}
function ExecuteFunctionByName(functionName, context /*, args */) {
    if (!functionName)
        return;
    var args = Array.prototype.slice.call(arguments, 2);
    var namespaces = functionName.split(".");
    var func = namespaces.pop();
    for (var i = 0; i < namespaces.length; i++) {
        context = context[namespaces[i]];
    }
    return context[func].apply(context, args);
}
function LoadDanhSachKhuonLen($el, Link, ListDataSelected) {
    GenDropDownListSelect2($el, Link || '/LenKhuonEp/LayDanhSachListKhuonSelect2', ListDataSelected, "Chọn khuôn lên", false, 20, true);
}
function LoadDanhSachKhuonXuong($el, Link, ListDataSelected) {
    GenDropDownListSelect2($el, Link || '/LenKhuonEp/LayDanhSachListKhuonSelect2', ListDataSelected, "Chọn khuôn xuống", false, 20, true);
}
function LoadDanhSachMay($el, Link, ListDataSelected) {
    GenDropDownListSelect2($el, Link || '/LenKhuonEp/ListMaySelect2', ListDataSelected, "Chọn máy", false, 20, true);
}
function LoadDanhSachNguyenLieu($el, Link, ListDataSelected) {
    GenDropDownListSelect2($el, Link || '/LenKhuonEp/ListNguyenLieuSelect2', ListDataSelected, "Chọn nguyên liệu", false, 20, true);
}
function GenDropDownListSelect2($Control, Url, ListSelected, Placeholder = "", AllowClear = true, pageSize = 20, trgChange = true, changeFunction = null, dataAttrName = null) {
    $Control.each(function () {

        var selected = [];
        var initials = [];
        var isHaveSelected = false;

        // debugger

        if ($.isArray(ListSelected)) {
            $.each(ListSelected, function (i, s) {
                //console.log(s);
                initials.push({ id: s.id, text: s.text, textextra: s.textextra, dataAttr: s.dataAttr });
                selected.push(s.id);
            });
        } else {
            if (ListSelected) {
                initials.push({ id: ListSelected?.id, text: ListSelected?.text, textextra: ListSelected?.textextra, dataAttr: ListSelected?.dataAttr });
                selected.push(ListSelected?.id);
            }
        }

        let $this = $(this);

        //console.log(this.name);

        //console.log($('option[selected="selected"]', this));

        $this.find('option[selected="selected"]').each(function () {
            //alert($(this).attr('id') + ':' + $(this).val());
            //debugger
            initials.push({ id: $(this).val(), text: $(this).text(), textextra: $(this).attr('textextra'), dataAttr: $(this).data('fielddataattr') });
            selected.push($(this).val());
            isHaveSelected = true;
        });

        if ($this.data('select2')) {
            $this.select2('destroy');
            $this.empty();
            //return;
        }

        $this.select2({
            data: initials,
            ajax: {
                delay: 200,
                url: Url,
                tokenSeparators: [";"],
                data: function (params) {
                    params.page = params.page || 1;
                    return {
                        searchTerm: params.term,
                        pageSize: pageSize || 20,
                        pageNumber: params.page
                    };
                },
                processResults: function (data, params) {
                    params.page = params.page || 1;
                    return {
                        results: data.Results,
                        pagination: {
                            more: (params.page * pageSize) < data.Total
                        }
                    };
                },
                cache: true,
            },
            tokenSeparators: [";"],
            placeholder: Placeholder,
            dropdownParent: $(this).parent(),
            minimumInputLength: 0,
            allowClear: AllowClear,
            templateResult: function (item) {

                if (dataAttrName != null)
                    $(item.element).attr("data-" + dataAttrName, item.dataAttr);

                if (item.textextra)
                    return $("<div>" + item.text + "<br /><small class='notDisplayChosed' style='color: brown;'>" + item.textextra + "<small></div>");
                else
                    return item.text;
                //return $("<div>"+item.text + "<br /><small style='color: #a5a0a0;'>Nhân Viên Bán Hàng<small></div>");
            },
            templateSelection: function (item) {
                if (dataAttrName != null)
                    $(item.element).attr("data-" + dataAttrName, item.dataAttr);
                return item.text;
            },
            //matcher: function (term, text) { return text.text.toUpperCase().indexOf(term.toUpperCase()) != -1; },
        });
        // Fetch the preselected item, and add to the control
        //if (isHaveSelected == true) {
        //    $.ajax({
        //        type: 'GET',
        //        url: Url,
        //        data: {
        //            searchTerm: '',
        //            valueSelected: selected.join(";"),
        //        }

        //    }).then(function (data) {
        //        // create the option and append to Select2

        //        $this.empty();

        //        let resutls = data.Results;

        //        if ($.isArray(resutls)) {
        //            $.each(resutls, function (i, s) {
        //                var option = new Option(s.text, s.id, true, true);
        //                if (dataAttrName != null)
        //                    $(option).attr("data-" + dataAttrName, s.dataAttr);
        //                $this.append(option);
        //            });
        //        } else {
        //            if (ListSelected) {
        //                var option = new Option(resutls.text, resutls.id, true, true);
        //                if (dataAttrName != null)
        //                    $(option).attr("data-" + dataAttrName, s.dataAttr);
        //                $this.append(option);
        //            }
        //        }

        //        $this.trigger('change')

        //        // manually trigger the `select2:select` event
        //        $this.trigger({
        //            type: 'select2:select',
        //            params: {
        //                data: resutls
        //            }
        //        });
        //    });
        //}

        if (changeFunction != null)
            changeFunction();

        // $Control.val(selected).trigger('change');

        if (selected != null) {
            if (trgChange == true)
                $this.val(selected).trigger('change');
            else $this.val(selected);
        }
    });
}