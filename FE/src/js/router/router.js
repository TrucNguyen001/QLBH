import { createRouter, createWebHistory } from "vue-router";
import Admin from "../../componentsweb/baseadmin/MainWeb.vue";
import Product from "../../view/admin/product/Product.vue";
import ProductType from "../../view/admin/producttype/ProductType.vue";
import ManageUser from "../../view/admin/user/User.vue";
import DiscountCode from "../../view/admin/discountcode/DiscountCode.vue";
import Invoice from "../../view/admin/invoice/Invoice.vue";
import News from "../../view/admin/news/News.vue";
import Supplier from "../../view/admin/supplier/Supplier.vue";
import OverView from "../../view/admin/home/OverView.vue";
import FeedBack from "@/view/admin/feedback/FeedBack.vue";

import User from "../../componentsweb/baseuser/MainWeb.vue";
import Home from "@/view/user/home/Home.vue";
import ListProduct from "@/view/user/listproduct/ListProduct.vue";
import ProductDetail from "@/view/user/detailproduct/ProductDetail.vue";
import Cart from "../../view/user/cart/Cart.vue";
import ConfirmInfoUser from "../../view/user/cart/ConfirmInfoUser.vue";
import UserInvoice from "../../view/user/invoice/Invoice.vue";

import InvoiceDetail from "../../view/admin/invoice/InoivceDetail.vue";

import LoginUser from "../../view/loginuser/LoginApp.vue";

import LoginAdmin from "../../view/loginadmin/LoginApp.vue";

import Forget from "../../view/forgetpassword/RecoverPassword.vue";
import InfoCompany from "../../view/user/info/InfoCompany.vue";
import UserNews from "../../view/user/news/News.vue";
import UserNewsDetail from "../../view/user/news/NewsDetail.vue";

import notfound from "../../view/notfound/NotFound.vue";

import ImportFile from "../../view/importfile/ImportFile.vue";

const routes = [
  {
    path: "/admin",
    component: Admin,
    meta: { requiresAuth: true },
    children: [
      {
        path: "overview",
        component: OverView,
      },
      {
        path: "product",
        component: Product,
      },
      {
        path: "product-type",
        component: ProductType,
      },
      {
        path: "user",
        component: ManageUser,
      },
      {
        path: "discount-code",
        component: DiscountCode,
      },
      {
        path: "invoice",
        component: Invoice,
      },
      {
        path: "news",
        component: News,
      },
      {
        path: "supplier",
        component: Supplier,
      },
      {
        path: "feedback",
        component: FeedBack,
      },
    ],
  },
  {
    path: "/user",
    component: User,
    children: [
      {
        path: "/",
        component: Home,
      },
      {
        path: "list-product",
        component: ListProduct,
      },
      {
        path: "product-detail/:id",
        component: ProductDetail,
      },
      {
        path: "cart",
        component: Cart,
      },
      {
        path: "invoice",
        component: UserInvoice,
      },
      {
        path: "info",
        component: InfoCompany,
      },
      {
        path: "news",
        component: UserNews,
      },
      {
        path: "news-detail/:id",
        component: UserNewsDetail,
      },
    ],
  },
  {
    path: "/login-user",
    component: LoginUser,
  },
  {
    path: "/login-admin",
    component: LoginAdmin,
  },
  {
    path: "/forget",
    component: Forget,
  },
  {
    path: "/confirm-info-user",
    component: ConfirmInfoUser,
  },
  {
    path: "/invoice-detail/:id",
    component: InvoiceDetail,
  },
  {
    path: "/import-file",
    component: ImportFile,
  },
  {
    path: "/:pathMatch(.*)*",
    component: notfound,
  },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

//Kiểm tra trạng thái đăng nhập trước mỗi lần chuyển route
router.beforeEach((to, from, next) => {
  let loggedIn = localStorage.getItem("isLogin"); // Kiểm tra trạng thái đăng nhập từ local storage

  if (
    to.matched.some((record) => record.meta.requiresAuth) &&
    loggedIn !== "true"
  ) {
    if (to.path.includes("/admin")) {
      next("/login-admin");
    } else {
      next("/login-user");
    }
  } else {
    next(); // Nếu đã đăng nhập hoặc không yêu cầu đăng nhập, cho phép truy cập
  }
});

export default router;
