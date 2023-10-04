import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';

import { history, fetchWrapper } from '_helpers';

// create slice

const name = 'signup';
const initialState = createInitialState();
const reducers = createReducers();
const extraActions = createExtraActions();
const extraReducers = createExtraReducers();
const slice = createSlice({ name, initialState, reducers, extraReducers });

// exports

export const signUpActions = { ...slice.actions, ...extraActions };
export const signUpReducer = slice.reducer;

// implementation

function createInitialState() {
    return {
        // initialize state from local storage to enable user to stay logged in
        user: JSON.parse(localStorage.getItem('user')),
        error: null
    }
}

function createReducers() {
    return {
        logout
    };

    function logout(state) {
        state.user = null;
        localStorage.removeItem('user');
        history.navigate('/login');
    }
}

function createExtraActions() {
    const baseUrl = `${process.env.REACT_APP_API_URL}/auth`;

    return {
        signup: signup()
    };    

    function signup() {
        return createAsyncThunk(
            `${name}/signup`,
            async ({ token }) => await fetchWrapper.post(`${baseUrl}/signup-google`, { token })
        );
    }
}

function createExtraReducers() {
    return {
        ...signup()
    };

    function signup() {
        var { pending, fulfilled, rejected } = extraActions.signup;
        return {
            [pending]: (state) => {
                state.error = null;
            },
            [fulfilled]: (state, action) => {
                history.navigate('/onboarding');
            },
            [rejected]: (state, action) => {
                state.error = action.error;
            }
        };
    }
}
