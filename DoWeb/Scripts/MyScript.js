function initScript() {
    //tính chênh lệch cho từng dữ liệu
    $("[datagroup]").each(function () {
        const group = $(this);

        group.find(".chuan, .thucte").on("input", function () {
            const chuan = parseFloat(group.find(".chuan").val()) || 0;
            const thucte = parseFloat(group.find(".thucte").val()) || 0;
            const chenhlech = thucte - chuan;

            group.find(".chenhlech").val(chenhlech.toFixed(2));
        });
    });
    // Tính điểm trung bình và xếp loại
    $(document).ready(function () {
        function tinhDiemTB() {
            let sum = 0, count = 0;
            $(".inputSmallNumber").each(function () {
                let val = parseFloat($(this).val());
                if (!isNaN(val) && val >= 1 && val <= 5) {
                    sum += val;
                    count++;
                }
            });

            if (count > 0) {
                let avg = (sum / count).toFixed(2);
                $("#DiemTB").val(avg);

                let xeploai = "";
                if (avg >= 4.5) xeploai = "Giỏi";
                else if (avg >= 3.5) xeploai = "Khá";
                else if (avg >= 2.5) xeploai = "Trung bình";
                else if (avg >= 1.5) xeploai = "Không đạt";
                else xeploai = "Kém";

                $("#XepLoai").val(xeploai);
            } else {
                $("#DiemTB").val("");
                $("#XepLoai").val("");
            }
        }

        $(document).on("input", ".inputSmallNumber", function () {
            let val = parseFloat($(this).val());
            if (isNaN(val) || val < 1 || val > 5) $(this).val('');
            tinhDiemTB();
        });
    });
    // Validate cho class inputNumber từ 1 đến 5
    $(document).ready(function () {
        $(".inputSmallNumber").on("input", function () {
            let val = parseInt($(this).val());
            if (isNaN(val) || val < 1 || val > 5) {
                $(this).val('');
            }
        });
    });
    // Ngăn chặn submit form khi nhấn Enter
    $(document).on("keydown", "form input", function (e) {
        if (e.key === "Enter") {
            e.preventDefault();
            return false;
        }
    });
    // Tính tổng và chênh lệch
    $(document).ready(function () {
        function tinhTongVaChenh() {
            const soShotPX = parseFloat($("#SoShotPX").val()) || 0;
            const soShotPRH = parseFloat($("#SoShotPRH").val()) || 0;
            const dinhMuc = parseFloat($("#DinhMuc").val()) || 0;

            const tong = soShotPX + soShotPRH;
            const chenhLech = tong - dinhMuc;

            $("#Tong").val(tong);
            $("#ShotChenhLech").val(chenhLech);
        }

        $("#SoShotPX, #SoShotPRH, #DinhMuc").on("input", tinhTongVaChenh);
    });
    // Chỉ cho phép nhập số nguyên từ 0 đến 999999
    $(document).on('input', '.inputInt', function () {
        this.value = this.value.replace(/[^0-9]/g, '');
        let val = parseInt($(this).val());
        if (isNaN(val) || val < 0 || val > 999999) {
            $(this).val('');
        }
    });

    // Chỉ cho phép nhập số thập phân từ 0 đến 10000
    $(document).on('input', '.inputDecimal', function () {
        let value = this.value;
        value = value.replace(',', '.');
        value = value.replace(/[^0-9.]/g, '').replace(/(\..*)\./g, '$1');
        if (value.startsWith('.')) {
            value = '0' + value;
        }
        const num = parseFloat(value);
        if (!isNaN(num)) {
            if (num < 0 || num > 10000) {
                value = '';
            }
        }
        this.value = value;
    });
    // Xử lý submit form trong popup
    $(document).on("submit", "form", function (e) {
        e.preventDefault();
        var form = $(this);

        form.find(".text-danger").remove();

        $.ajax({
            type: form.attr("method"),
            url: form.attr("action"),
            data: form.serialize(),
            success: function (res) {
                if (res.success) {
                    $("#formModal").modal("hide");
                    alert(res.message);
                    location.reload();
                } else if (res.errors) {
                    for (var key in res.errors) {
                        var input = form.find("[name='" + key + "']");
                        if (input.length) {
                            input.after(
                                '<span class="text-danger small">' + res.errors[key][0] + "</span>"
                            );
                        }
                    }
                } else {
                    alert(res.message || "Có lỗi xảy ra");
                }
            },
            error: function () {
                alert("Lỗi kết nối đến server");
            }
        });
    });
}





