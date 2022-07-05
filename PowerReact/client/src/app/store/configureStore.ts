import {configureStore} from "@reduxjs/toolkit";
import {basketSlice} from "../../features/basket/basketSlice";
import {TypedUseSelectorHook, useDispatch, useSelector} from "react-redux";
import {catalogSlice} from "../../features/catalog/catalogSlice";
import {accountSlice} from "../../features/account/accountSlice";


// This makes the variable basket visible to the rest of the app under where the <Provider store={store}> was added

export const store = configureStore({
    reducer: {
        basket: basketSlice.reducer,
        catalog: catalogSlice.reducer,
        account: accountSlice.reducer
    }
})


// root state is just store state
export type RootState = ReturnType<typeof  store.getState>
export const useAppSelector : TypedUseSelectorHook<RootState> = useSelector;


// If we want to call the basket variable in a certain component we would use
// const  { basket } = useSelector((state: ReturnType<typeof  store.getState>) => state.basket)
// const dispatch = useDispatch()
// dispatch(redicer fimctopm)

export type AppDispatch = typeof store.dispatch;
export const useAppDispatch = () => useDispatch<AppDispatch>()
