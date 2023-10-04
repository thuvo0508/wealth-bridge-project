import { configureStore } from '@reduxjs/toolkit';

import { authReducer } from './auth.slice';
import { usersReducer } from './users.slice';
import { signUpReducer } from './signup.slice';

export * from './auth.slice';
export * from './users.slice';
export * from './signup.slice';

export const store = configureStore({
    reducer: {
        auth: authReducer,
        users: usersReducer,
        signup: signUpReducer
    },
});