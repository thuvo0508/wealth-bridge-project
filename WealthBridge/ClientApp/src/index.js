import React from 'react';
import { createRoot } from 'react-dom/client';
import { Provider } from 'react-redux';
import { BrowserRouter } from 'react-router-dom';
import { GoogleOAuthProvider } from '@react-oauth/google';

import { store } from './_store';
import { App } from './App';
import './index.css';

// setup fake backend
// import { fakeBackend } from './_helpers';
// fakeBackend();

const container = document.getElementById('root');
const root = createRoot(container);

root.render(
    <React.StrictMode>
        <Provider store={store}>
            <BrowserRouter>
                <GoogleOAuthProvider clientId="1087798691818-lqsmtj17ttk5bk1rfut31tio74sinuvd.apps.googleusercontent.com">
                    <App />
                </GoogleOAuthProvider>
            </BrowserRouter>
        </Provider>
    </React.StrictMode>
);
