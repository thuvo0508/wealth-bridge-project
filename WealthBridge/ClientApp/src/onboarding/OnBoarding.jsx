import { useEffect } from 'react';
import { useForm } from "react-hook-form";
import { yupResolver } from '@hookform/resolvers/yup';
import * as Yup from 'yup';
import { useSelector, useDispatch } from 'react-redux';

import { history } from '_helpers';
import { authActions } from '_store';

import 'css/vendors/flatpickr.min.css';
import '../style.css';
import onBoardingImage from '../images/onboarding-image.jpg';
import authDecoration from '../images/auth-decoration.png';

export { OnBoarding };

function OnBoarding() {
    const dispatch = useDispatch();
    const authUser = useSelector(x => x.auth.user);

    useEffect(() => {
        // redirect to home if already logged in
        if (authUser) history.navigate('/');

        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    // // form validation rules 
    // const validationSchema = Yup.object().shape({
    //     username: Yup.string().required('Username is required'),
    //     password: Yup.string().required('Password is required')
    // });
    // const formOptions = { resolver: yupResolver(validationSchema) };

    // // get functions to build form with useForm() hook
    // const { register, handleSubmit, formState } = useForm(formOptions);
    // const { errors, isSubmitting } = formState;

    // function onSubmit({ username, password }) {
    //     return dispatch(authActions.login({ username, password }));
    // }

    return (
      <main className="bg-white dark:bg-slate-900">
      <div className="relative flex">
        {/* <!-- Content --> */}
        <div className="w-full md:w-1/2">
          <div className="min-h-[100dvh] h-full flex flex-col after:flex-1">
            <div className="flex-1">
              {/* <!-- Header --> */}
              <div
                className="flex items-center justify-between h-16 px-4 sm:px-6 lg:px-8"
              >
                {/* <!-- Logo --> */}
                <a className="block" href="index.html">
                  <svg width="32" height="32" viewBox="0 0 32 32">
                    <defs>
                      <linearGradient
                        x1="28.538%"
                        y1="20.229%"
                        x2="100%"
                        y2="108.156%"
                        id="logo-a"
                      >
                        <stop
                          stopColor="#A5B4FC"
                          stopOpacity="0"
                          offset="0%"
                        />
                        <stop stopColor="#A5B4FC" offset="100%" />
                      </linearGradient>
                      <linearGradient
                        x1="88.638%"
                        y1="29.267%"
                        x2="22.42%"
                        y2="100%"
                        id="logo-b"
                      >
                        <stop
                          stopColor="#38BDF8"
                          stopOpacity="0"
                          offset="0%"
                        />
                        <stop stopColor="#38BDF8" offset="100%" />
                      </linearGradient>
                    </defs>
                    <rect fill="#6366F1" width="32" height="32" rx="16" />
                    <path
                      d="M18.277.16C26.035 1.267 32 7.938 32 16c0 8.837-7.163 16-16 16a15.937 15.937 0 01-10.426-3.863L18.277.161z"
                      fill="#4F46E5"
                    />
                    <path
                      d="M7.404 2.503l18.339 26.19A15.93 15.93 0 0116 32C7.163 32 0 24.837 0 16 0 10.327 2.952 5.344 7.404 2.503z"
                      fill="url(#logo-a)"
                    />
                    <path
                      d="M2.223 24.14L29.777 7.86A15.926 15.926 0 0132 16c0 8.837-7.163 16-16 16-5.864 0-10.991-3.154-13.777-7.86z"
                      fill="url(#logo-b)"
                    />
                  </svg>
                </a>
                <div className="text-sm">
                  Have an account?
                  <a
                    className="font-medium text-indigo-500 hover:text-indigo-600 dark:hover:text-indigo-400"
                    href="login"
                    >Sign In</a
                  >
                </div>
              </div>

              {/* <!-- Progress bar --> */}
              <div className="px-4 pt-12 pb-8">
                <div className="w-full max-w-md mx-auto">
                  <div className="relative">
                    <div
                      className="absolute left-0 top-1/2 -mt-px w-full h-0.5 bg-slate-200 dark:bg-slate-700"
                      aria-hidden="true"
                    ></div>
                    <ul className="relative flex justify-between w-full">
                      <li>
                        <a
                          className="flex items-center justify-center w-6 h-6 text-xs font-semibold text-white bg-indigo-500 rounded-full"
                          href="onboarding-03.html"
                          >1</a
                        >
                      </li>
                      <li>
                        <a
                          className="flex items-center justify-center w-6 h-6 text-xs font-semibold text-white bg-indigo-500 rounded-full"
                          href="onboarding-04.html"
                          >2</a
                        >
                      </li>
                    </ul>
                  </div>
                </div>
              </div>
            </div>

            <div className="px-4 py-8">
              <div className="max-w-md mx-auto">
                <div className="text-center">
                  <svg
                    className="inline-flex w-16 h-16 mb-6 fill-current"
                    viewBox="0 0 64 64"
                  >
                    <circle
                      className="text-emerald-100 dark:text-emerald-400/30"
                      cx="32"
                      cy="32"
                      r="32"
                    />
                    <path
                      className="text-emerald-500 dark:text-emerald-400"
                      d="m28.5 41-8-8 3-3 5 5 12-12 3 3z"
                    />
                  </svg>
                  <h1
                    className="mb-8 text-2xl font-bold text-slate-800 dark:text-slate-100"
                  >
                    Great to have you on board! <br />
                    A representative will be reaching out shortly with next
                    steps.
                  </h1>
                  <a
                    className="text-white bg-indigo-500 btn hover:bg-indigo-600"
                    href="dashboard.html"
                    >Go To Dashboard -&gt;</a
                  >
                </div>
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
            src={onBoardingImage}
            width="760"
            height="1024"
            alt="Onboarding"
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
