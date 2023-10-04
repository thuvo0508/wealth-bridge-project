import { useEffect } from 'react';
import { useForm } from "react-hook-form";
import { yupResolver } from '@hookform/resolvers/yup';
import * as Yup from 'yup';
import { useSelector, useDispatch } from 'react-redux';
import { useGoogleLogin } from '@react-oauth/google';
import axios from 'axios';

import { history } from '_helpers';
import { authActions } from '_store';

import 'css/vendors/flatpickr.min.css';
import '../style.css';
import authImage from '../images/auth-image.jpg';
import authDecoration from '../images/auth-decoration.png';

export { Login };

function Login() {
    const dispatch = useDispatch();
    const authUser = useSelector(x => x.auth.user);

    useEffect(() => {
        // redirect to home if already logged in
        if (authUser) history.navigate('/');
    }, []);

    const signInGoogle = useGoogleLogin({
      onSuccess: codeResponse => {
        var token = codeResponse.access_token;
        return dispatch(authActions.login({token}));
      },
      onError: () => {
        console.log('Login Failed');
      }
    });

    return (
<main className="bg-white dark:bg-slate-900">
      <div className="relative flex">
        {/* <!-- Content --> */}
        <div className="w-full md:w-1/2">
          <div className="min-h-[100dvh] h-full flex flex-col after:flex-1">
            {/* <!-- Header --> */}
            <div className="flex-1">
              <div
                className="flex items-center justify-between h-16 px-4 sm:px-6 lg:px-8"
              >
              </div>
            </div>

            <div className="w-full max-w-sm px-4 py-8 mx-auto">
              <h1
                className="mb-6 text-3xl font-bold text-slate-800 dark:text-slate-100"
              >
                Welcome back! ✨
              </h1>
              {/* <!-- Form --> */}
              {/* <!-- <form>
                <div className="space-y-4">
                  <div>
                    <label className="block mb-1 text-sm font-medium" for="email"
                      >Email Address</label
                    >
                    <input id="email" className="w-full form-input" type="email" />
                  </div>
                  <div>
                    <label className="block mb-1 text-sm font-medium" for="password"
                      >Password</label
                    >
                    <input
                      id="password"
                      className="w-full form-input"
                      type="password"
                      autocomplete="on"
                    />
                  </div>
                </div>
                <div className="flex items-center justify-between mt-6">
                  <div className="mr-1">
                    <a
                      className="text-sm underline hover:no-underline"
                      href="reset-password.html"
                      >Forgot Password?</a
                    >
                  </div>
                  <a
                    className="ml-3 text-white bg-indigo-500 btn hover:bg-indigo-600"
                    href="dashboard.html"
                    >Sign In</a
                  >
                </div>
              </form> --> */}
              {/* <!-- Sign In with third party --> */}
              <div className="flex flex-col max-w-2xl mx-auto mt-6">
                <button
                  type="button"
                  className="text-white bg-[#3b5998] hover:bg-[#3b5998]/90 focus:ring-4 focus:ring-[#3b5998]/50 font-medium rounded-lg text-sm px-5 py-2.5 text-center flex items-center justify-center dark:focus:ring-[#3b5998]/55 mb-3"
                >
                  <svg
                    className="w-4 h-4 mr-2 -ml-1"
                    aria-hidden="true"
                    focusable="false"
                    data-prefix="fab"
                    data-icon="facebook-f"
                    role="img"
                    xmlns="http://www.w3.org/2000/svg"
                    viewBox="0 0 320 512"
                  >
                    <path
                      fill="currentColor"
                      d="M279.1 288l14.22-92.66h-88.91v-60.13c0-25.35 12.42-50.06 52.24-50.06h40.42V6.26S260.4 0 225.4 0c-73.22 0-121.1 44.38-121.1 124.7v70.62H22.89V288h81.39v224h100.2V288z"
                    ></path>
                  </svg>
                  Sign in with Facebook
                </button>
                <button
                  type="button"
                  className="text-white bg-[#1da1f2] hover:bg-[#1da1f2]/90 focus:ring-4 focus:ring-[#1da1f2]/50 font-medium rounded-lg text-sm px-5 py-2.5 text-center flex items-center justify-center dark:focus:ring-[#1da1f2]/55 mb-3"
                >
                  <svg
                    className="w-4 h-4 mr-2 -ml-1"
                    aria-hidden="true"
                    focusable="false"
                    data-prefix="fab"
                    data-icon="twitter"
                    role="img"
                    xmlns="http://www.w3.org/2000/svg"
                    viewBox="0 0 512 512"
                  >
                    <path
                      fill="currentColor"
                      d="M459.4 151.7c.325 4.548 .325 9.097 .325 13.65 0 138.7-105.6 298.6-298.6 298.6-59.45 0-114.7-17.22-161.1-47.11 8.447 .974 16.57 1.299 25.34 1.299 49.06 0 94.21-16.57 130.3-44.83-46.13-.975-84.79-31.19-98.11-72.77 6.498 .974 12.99 1.624 19.82 1.624 9.421 0 18.84-1.3 27.61-3.573-48.08-9.747-84.14-51.98-84.14-102.1v-1.299c13.97 7.797 30.21 12.67 47.43 13.32-28.26-18.84-46.78-51.01-46.78-87.39 0-19.49 5.197-37.36 14.29-52.95 51.65 63.67 129.3 105.3 216.4 109.8-1.624-7.797-2.599-15.92-2.599-24.04 0-57.83 46.78-104.9 104.9-104.9 30.21 0 57.5 12.67 76.67 33.14 23.72-4.548 46.46-13.32 66.6-25.34-7.798 24.37-24.37 44.83-46.13 57.83 21.12-2.273 41.58-8.122 60.43-16.24-14.29 20.79-32.16 39.31-52.63 54.25z"
                    ></path>
                  </svg>
                  Sign in with Twitter
                </button>
                <button
                  type="button"
                  className="text-white bg-[#4285F4] hover:bg-[#4285F4]/90 focus:ring-4 focus:ring-[#4285F4]/50 font-medium rounded-lg text-sm px-5 py-2.5 text-center flex items-center justify-center dark:focus:ring-[#4285F4]/55 mb-3"
                  onClick={() => signInGoogle()}
                >
                  <svg
                    className="w-4 h-4 mr-2 -ml-1"
                    aria-hidden="true"
                    focusable="false"
                    data-prefix="fab"
                    data-icon="google"
                    role="img"
                    xmlns="http://www.w3.org/2000/svg"
                    viewBox="0 0 488 512"
                  >
                    <path
                      fill="currentColor"
                      d="M488 261.8C488 403.3 391.1 504 248 504 110.8 504 0 393.2 0 256S110.8 8 248 8c66.8 0 123 24.5 166.3 64.9l-67.5 64.9C258.5 52.6 94.3 116.6 94.3 256c0 86.5 69.1 156.6 153.7 156.6 98.2 0 135-70.4 140.8-106.9H248v-85.3h236.1c2.3 12.7 3.9 24.9 3.9 41.4z"
                    ></path>
                  </svg>
                  Sign in with Google
                </button>
                <button
                  type="button"
                  className="text-white bg-[#7CBB00] hover:bg-[#7CBB00]/90 focus:ring-4 focus:ring-[#7CBB00]/50 font-medium rounded-lg text-sm px-5 py-2.5 text-center flex items-center justify-center dark:focus:ring-[#7CBB00]/55 mb-3"
                >
                  <svg
                    className="w-4 h-4 mr-2 -ml-1"
                    stroke="currentColor"
                    fill="currentColor"
                    strokeWidth="0"
                    viewBox="0 0 16 16"
                    height="1em"
                    width="1em"
                    xmlns="http://www.w3.org/2000/svg"
                  >
                    <path
                      d="M7.462 0H0v7.19h7.462V0zM16 0H8.538v7.19H16V0zM7.462 8.211H0V16h7.462V8.211zm8.538 0H8.538V16H16V8.211z"
                    ></path>
                  </svg>
                  Sign in with Microsoft
                </button>
                <button
                  type="button"
                  className="text-white bg-[#050708] hover:bg-[#050708]/90 focus:ring-4 focus:ring-[#050708]/50 font-medium rounded-lg text-sm px-5 py-2.5 text-center flex items-center justify-center dark:focus:ring-[#050708]/50 dark:hover:bg-[#050708]/30 mr-2 mb-2"
                >
                  <svg
                    className="w-5 h-5 mr-2 -ml-1"
                    aria-hidden="true"
                    focusable="false"
                    data-prefix="fab"
                    data-icon="apple"
                    role="img"
                    xmlns="http://www.w3.org/2000/svg"
                    viewBox="0 0 384 512"
                  >
                    <path
                      fill="currentColor"
                      d="M318.7 268.7c-.2-36.7 16.4-64.4 50-84.8-18.8-26.9-47.2-41.7-84.7-44.6-35.5-2.8-74.3 20.7-88.5 20.7-15 0-49.4-19.7-76.4-19.7C63.3 141.2 4 184.8 4 273.5q0 39.3 14.4 81.2c12.8 36.7 59 126.7 107.2 125.2 25.2-.6 43-17.9 75.8-17.9 31.8 0 48.3 17.9 76.4 17.9 48.6-.7 90.4-82.5 102.6-119.3-65.2-30.7-61.7-90-61.7-91.9zm-56.6-164.2c27.3-32.4 24.8-61.9 24-72.5-24.1 1.4-52 16.4-67.9 34.9-17.5 19.8-27.8 44.3-25.6 71.9 26.1 2 49.9-11.4 69.5-34.3z"
                    ></path>
                  </svg>
                  Sign in with Apple
                </button>
              </div>
              {/* <!-- Footer --> */}
              <div
                className="pt-5 mt-6 border-t border-slate-200 dark:border-slate-700"
              >
                <div className="text-sm">
                  Don’t you have an account?
                  <a
                    className="font-medium text-indigo-500 hover:text-indigo-600 dark:hover:text-indigo-400"
                    href="/signup"
                    >Sign Up</a
                  >
                </div>
                {/* <!-- Warning --> */}
                {/* <!-- <div className="mt-5">
                                <div className="px-3 py-2 rounded bg-amber-100 dark:bg-amber-400/30 text-amber-600 dark:text-amber-400">
                                    <svg className="inline w-3 h-3 fill-current shrink-0" viewBox="0 0 12 12">
                                        <path d="M10.28 1.28L3.989 7.575 1.695 5.28A1 1 0 00.28 6.695l3 3a1 1 0 001.414 0l7-7A1 1 0 0010.28 1.28z" />
                                    </svg>
                                    <span className="text-sm">
                                        To support you during the pandemic super pro features are free until March 31st.
                                    </span>
                                </div>
                            </div> --> */}
              </div>
            </div>
          </div>
        </div>

        {/* <!-- Image --> */}
        <div
          className="absolute top-0 bottom-0 right-0 hidden md:block md:w-1/2"
          aria-hidden="true"
        >
          <img
            className="object-cover object-center w-full h-full"
            src={authImage}
            width="760"
            height="1024"
            alt="Authentication image"
          />
          <img
            className="absolute left-0 hidden ml-8 -translate-x-1/2 top-1/4 lg:block"
            src={authDecoration}
            width="218"
            height="224"
            alt="Authentication decoration"
          />
        </div>
      </div>
    </main>

     
    )
}
