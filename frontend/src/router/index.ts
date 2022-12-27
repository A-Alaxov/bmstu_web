import Vue from 'vue'
import VueRouter, { RouteConfig } from 'vue-router'
import Login_page from '@/views/Login.vue';
import projectSelection from '@/views/Project_selection.vue';
import projectView from '@/views/Project_view.vue';
import Registration_page from '@/views/Registration.vue';
import workplaceSelection from '@/views/Workplace_selection.vue';
import employeesView from '@/views/Employees_view.vue';
import departmentsView from '@/views/Departments_view.vue';
import userView from '@/views/User_view.vue';
import workplaceView from '@/views/Workplace_view.vue';

Vue.use(VueRouter)

const routes: Array<RouteConfig> = [
  {
    path: '/login',
    name: 'Login',
    component: Login_page,
  },
  {
    path: '/registration',
    name: 'Registration',
    component: Registration_page,
  },
  {
    path: '/projects',
    name: 'projectSelection',
    component: projectSelection,
    meta: { authorize: [] }
  },
  {
    path: '/projectView',
    name: 'projectView',
    component: projectView,
    meta: { authorize: [] }
  },
  {
    path: '/employeesView',
    name: 'employeesView',
    component: employeesView,
    meta: { authorize: [] }
  },
  {
    path: '/workplaceSelection',
    name: 'workplaceSelection',
    component: workplaceSelection,
    meta: { authorize: [] }
  },
  {
    path: '/departmentsView',
    name: 'departmentsView',
    component: departmentsView,
    meta: { authorize: [] }
  },
  {
    path: '/userView',
    name: 'userView',
    component: userView,
    meta: { authorize: [] }
  },
  {
    path: '/workplaceView',
    name: 'workplaceView',
    component: workplaceView,
    meta: { authorize: [] }
  },

  { path: '*', redirect: '/login' }
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})
/*
router.beforeEach((to, from, next) => {
  const currentUser = router.app.$store.getters.getUser

  if (to.path !== '/login' && to.path !== '/registration' && !currentUser) {
    return next({ path: "/login", query: { returnUrl: to.path } });
  }

  next();
});
*/
export default router
