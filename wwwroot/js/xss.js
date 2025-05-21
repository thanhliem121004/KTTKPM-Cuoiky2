// Hàm lọc và mã hóa dữ liệu đầu vào
function sanitizeInput(input) {
    // Chỉ mã hóa các ký tự đặc biệt HTML, giữ nguyên các ký tự Unicode
    return input.replace(/&/g, '&amp;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;')
        .replace(/"/g, '&quot;')
        .replace(/'/g, '&#039;');
}

// Hàm kiểm tra và lọc dữ liệu khi người dùng nhập
function validateAndSanitizeInput(inputElement) {
    inputElement.addEventListener('input', function () {
        // Lưu giá trị gốc
        this.dataset.originalValue = this.value;
        // Hiển thị giá trị đã được sanitize
        this.value = sanitizeInput(this.value);
    });

    inputElement.addEventListener('change', function () {
        // Khôi phục giá trị gốc khi focus ra khỏi input
        this.value = this.dataset.originalValue || '';
    });
}

// Áp dụng cho tất cả các input trên trang
document.addEventListener('DOMContentLoaded', function () {
    let inputs = document.getElementsByTagName('input');
    for (let i = 0; i < inputs.length; i++) {
        validateAndSanitizeInput(inputs[i]);
    }
});

// Hàm mã hóa dữ liệu đầu ra trước khi hiển thị
function encodeOutput(output) {
    let div = document.createElement('div');
    div.appendChild(document.createTextNode(output));
    return div.innerHTML;
}

// Sử dụng hàm mã hóa đầu ra
function displaySafeContent(content, elementId) {
    let element = document.getElementById(elementId);
    if (element) {
        element.innerHTML = encodeOutput(content);
    }
}