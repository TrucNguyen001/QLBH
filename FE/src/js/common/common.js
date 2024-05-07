import resource from "../helper/resource.js";
import helper from "../helper/helper.js";
import apiService from "../apiservice/apiservice.js";
import tinyEmitter from "tiny-emitter/instance";
let $ = require("jquery");
const common = {
  /**
   * Chuyển đổi ngày thành dạng ngày / tháng / năm
   * @param {ngày chuyển đổi} date
   * @returns Trả về ngày sau khi chuyển đổi xong
   * @author: Nguyễn Văn Trúc(6/3/2024)
   */
  changeDisplayDate(date) {
    try {
      let dateOfBirth = "";
      if (date) {
        date = new Date(date);
        let day = date.getDate();
        day = day < 10 ? `0${day}` : day;
        let month = date.getMonth() + 1;
        month = month < 10 ? `0${month}` : month;
        let year = date.getFullYear();
        dateOfBirth = `${day}/${month}/${year}`;
      }
      return dateOfBirth;
    } catch (error) {
      console.log(error);
    }
  },

  /**
   * Hàm Thực hiện khi thay đổi cách hiển thị tiền
   * @author: Nguyễn Văn Trúc (7/4/2024)
   */
  changeDisplayDebitAmount: function (debitAmount) {
    return new Intl.NumberFormat("vi-VN", {
      style: "currency",
      currency: "VND",
    }).format(debitAmount);
  },

  /**
   * Chuyển đổi giớI tính thành chữ
   * @param {Giới tính chuyển đổi(dạng số)} gender
   * @returns Trả về giớI tính sau chuyển đổi
   * @author: Nguyễn Văn Trúc(6/3/2024)
   */
  changeDisplayGender(gender) {
    try {
      return gender === 0
        ? resource.Gender[0]
        : gender === 1
        ? resource.Gender[1]
        : gender === null
        ? ""
        : resource.Gender[2];
    } catch (error) {
      console.log(error);
    }
  },

  /**
   * Hàm kiểm tra email
   * @param {Email muốn kiểm tra} email
   * @returns true: email đúng định dạng
   * false: Sai định dạng
   * @author: Nguyễn Văn Trúc(6/3/2024)
   */
  checkEmail(email) {
    try {
      if (email === null || email === "" || email === undefined) {
        return true;
      }
      let regex = /^([a-zA-Z0-9_.-])+@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
      if (!regex.test(email)) {
        return false;
      }
      return true;
    } catch (error) {
      console.log(error);
    }
  },

  /**
   * Kiểm tra số điện thoại
   * @param {Số điện thoại} phone
   */
  checkPhoneNumber(phone) {
    try {
      if (phone === null || phone === "" || phone === undefined) {
        return true;
      }

      let regex = /^\d{9,15}$/;

      if (!regex.test(phone)) {
        return false;
      }
      return true;
    } catch (error) {
      console.log(error);
    }
  },

  /**
   * Hàm kiểm tra mật khẩu
   * @param {Mật khẩu} password
   * @returns true: password đúng định dạng
   * false: password sai định dạng
   * @author: Nguyễn Văn Trúc(10/3/2024)
   */
  checkFormatPassword(password) {
    try {
      let regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$/;
      if (!regex.test(password)) {
        return false;
      } else {
        return true;
      }
    } catch (error) {
      console.log(error);
    }
  },

  /**
   * Hàm thay đổi hiển thị email
   * @param {email} email
   * @returns: trả về email sau thay đổi
   * @author: Nguyễn Văn Trúc(10/3/2024)
   */
  replaceEmailChars(email) {
    // Tìm vị trí của ký tự '@'
    let atIndex = email.indexOf("@");

    // Nếu không tìm thấy ký tự '@', trả về email ban đầu
    if (atIndex === -1) {
      return email;
    }

    // Lấy phần tiền tố từ đầu đến ký tự '@'
    let prefix = email.substring(0, 3);

    // Thay thế các ký tự bằng '*'
    let maskedChars = email.substring(3, atIndex).replace(/./g, "*");

    // Lấy phần hậu tố từ ký tự '@' đến hết chuỗi
    let suffix = email.substring(atIndex);

    // Kết hợp phần tiền tố, phần thay thế và phần hậu tố để tạo email mới
    let newEmail = prefix + maskedChars + suffix;

    return newEmail;
  },

  /**
   * Kiểm tra date có lớn hơn ngày hiện tại
   * @param {Giá trị date truyền vào} item
   * @author: Nguyễn Văn Trúc (9/12/2023)
   */
  validateInputDate(item) {
    try {
      if (item) {
        item = new Date(item);
        if (item > new Date()) {
          return true;
        }
      }
    } catch (error) {
      console.log(error);
    }
  },

  /**
   * Hàm kiểm tra giá trị input
   * @param {Giá trị của input truyền vào để kiểm tra} item
   * @author: Nguyễn Văn Trúc (9/12/2023)
   */
  validateInput(item) {
    if (item === null || item === "" || item === undefined) {
      return true;
    }
  },

  /**
   * Loại bỏ những phần tử bị trùng(chỉ xuất hiện 1 lần)
   * @param {mảng truyền vào} array
   * @returns Trả về mảng sau biến đổi
   */
  uniqueArray(array) {
    return array.filter((value, index, self) => {
      return self.indexOf(value) === index;
    });
  },

  /**
   * Bôi đen ô input search khi click vào
   * @param {Id truyền vào} id
   * @author: Nguyễn Văn Trúc (15/3/2024)
   */
  blackenInput(id) {
    try {
      document.getElementById(id).select();
    } catch (error) {
      console.log(error);
    }
  },

  /**
   * Hàm hiển thị toast
   * @param {nội dung} content
   * @param {icon} icon
   * @author: Nguyễn Văn Trúc(13/3/2024)
   */
  showToast(content) {
    $("body").append(`<div style="width: 480px" class="m-show-toast">
    <div class="m-toast load-data-success">
      <div class="m-toast-left">
        <div><i class="bi bi-check-lg text-success"></i></div>
        <div class="m-toast-content"><strong class="mx-2" style="color: #50B83C">Thành công!</strong>${content}</div>
      </div>
      <div class="m-toast-right">
      <div class="close-toast-load-data"><i class="bi bi-x-lg text-danger"></i></div>
      </div>
    </div>
  </div>`);

    // Đóng form
    $(".close-toast-load-data").on("click", function () {
      $(".m-show-toast").remove();
    });

    // Đóng form sau 1 giây
    setTimeout(function () {
      $(".m-show-toast").remove();
    }, 2000);
  },

  /**
   * Hàm hiển thị toast
   * @param {nội dung} content
   * @param {icon} icon
   * @author: Nguyễn Văn Trúc(13/3/2024)
   */
  showToastError(content) {
    $("body").append(`<div class="m-show-toast">
    <div class="m-toast load-data-success">
      <div class="m-toast-left">
        <div><i class="bi bi-exclamation-triangle text-danger"></i></div>
        <div class="m-toast-content"><strong style="color: Red" class="mx-2">Thất bại!</strong>${content}</div>
      </div>
      <div class="m-toast-right">
        <div class="close-toast-load-data"><i class="bi bi-x-lg text-danger"></i></div>
      </div>
    </div>
  </div>`);

    // Đóng form
    $(".close-toast-load-data").on("click", function () {
      $(".m-show-toast").remove();
    });

    // Đóng form sau 1 giây
    setTimeout(function () {
      $(".m-show-toast").remove();
    }, 1600);
  },

  /**
   * Hàm hiển thị loading
   * @param {trạng thái ẩn hiện loading} visible
   * @author: Nguyễn Văn Trúc(13/3/2024)
   */
  showLoading(visible = true) {
    if (visible) {
      $("body").append(`<div class="m-loading">
            <div class="icon-loading"></div>
        </div>`);
    } else {
      $(".m-loading").remove();
    }
  },

  /**
   * Hàm hiển thị thông báo dialog
   * @param {Nội dung thông báo} message
   * @param {Tiêu đề thông báo} title
   * @param {icon} icon
   * @param {Trạng thái thông báo} status
   * @param {Đường dẫn} path
   * @param {Giá trị} value
   * @param {Bản ghi} record
   *  @author: Nguyễn Văn Trúc(6/3/2024)
   */
  showDialog(
    message,
    title = resource.ErrorData.Title,
    status = helper.Status.Default,
    path = "",
    value = "",
    valueId = ""
  ) {
    // Nội dung hông báo lỗi
    let messageHtml = "";
    // Gán key để focus
    let emptyKeys = [];
    // Kiểm tra xem message có phải là object không
    if (typeof message === "object" && message !== null) {
      // Lấy mảng chứa các giá trị từ object và loại bỏ những giá trị null hoặc undefined hoặc có độ dài bằng 0
      let valuesArray = Object.values(message).filter(
        (value) => value !== null && value !== undefined && value.length > 0
      );

      // Nếu có ít nhất một giá trị tồn tại
      if (valuesArray.length > 0) {
        // Hiển thị mảng các giá trị
        messageHtml = "<ul>";
        valuesArray.forEach((value) => {
          messageHtml += `<li>${value}</li>`;
        });
        messageHtml += "</ul>";
      }

      for (let key in message) {
        // Kiểm tra xem độ dài của giá trị tương ứng với key > 0
        if (message[key].length > 0) {
          // Nếu có, thêm key vào mảng emptyKeys
          emptyKeys.push(key);
        }
      }
    } else {
      // Nếu message không phải là object, hoặc là null hoặc không phải object, chỉ hiển thị nó như thông thường
      messageHtml = `<div>${message}</div>`;
    }
    $("body").append(`
    <div class="show-dialog">
      <div class="dialog">
        <div class="header-dialog">
          <div class="header-dialog-title">${title}</div>
          <div class="header-dialog-close">
            <div class="cancel-delete close-dialog"><i class="bi bi-x-lg"></i></div>
          </div>
        </div>
        <div class="body-dialog">
          <div><i style="font-size: 32px; color: gold" class="bi bi-exclamation-triangle"></i></div>
          <div class="content-dialog">
            <div>${messageHtml}</div>
          </div>
        </div>
        <div class="footer-dialog" style="margin-top: 45px">
          <div class="footer-left-dialog"></div>
          <div class="dialog-button">
            <button class="close-dialog btn btn-danger px-4">Không</button>
            <button class="accept-dialog btn btn-success px-4">Đồng ý</button>
          </div>
        </div>
      </div>
    </div>
      `);

    switch (status) {
      case helper.Status.UpdateInvoiceFalse:
        {
          $(".accept-dialog").on("click", function () {
            let result = apiService.update(path, value, valueId);
            result.then((data) => {
              if (data === 1) {
                tinyEmitter.emit("Status", status);
              }
            });
            $(".show-dialog").remove();
          });
        }
        break;
      case helper.Status.Update:
        {
          $(".accept-dialog").on("click", function () {
            tinyEmitter.emit("UpdateInvoiceFalse", valueId);
            let result = apiService.update(path, value, valueId);
            result.then((data) => {
              if (data === 1) {
                tinyEmitter.emit("Status", status);
              }
            });
            $(".show-dialog").remove();
          });
        }
        break;
      case helper.Status.Delete:
        {
          $(".accept-dialog").on("click", function () {
            let result = apiService.delete(path, value);
            result.then((data) => {
              if (data === 1) {
                tinyEmitter.emit("Status", status);
              }
            });
            $(".show-dialog").remove();
          });
        }
        break;
      case helper.Status.DeleteMultiple:
        {
          $(".accept-dialog").on("click", function () {
            let result = apiService.deleteMultiple(path, value);
            result.then((data) => {
              if (data === 1) {
                tinyEmitter.emit("Status", status);
              }
            });
            $(".show-dialog").remove();
          });
        }
        break;
      default:
        {
          $(".accept-dialog").on("click", function () {
            // Ẩn dialog
            $(".show-dialog").remove();
            if (emptyKeys.length > 0) {
              document.getElementById(emptyKeys[0]).focus();
            }
          });
        }
        break;
    }

    // Đóng form
    $(".close-dialog").on("click", function () {
      $(".show-dialog").remove();
    });
  },
};
export default common;
