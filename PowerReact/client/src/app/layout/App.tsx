import React, {useCallback, useEffect, useState} from 'react';
import './App.css';
import {Container, createTheme, CssBaseline, ThemeProvider} from "@mui/material";
import Header from "./Header";
import Catalog from "../../features/catalog/Catalog";
import {Route, Routes} from "react-router-dom";
import HomePage from "../../features/home/HomePage";
import AboutPage from "../../features/about/AboutPage";
import ContactPage from "../../features/contact/ContactPage";
import ProductDetails from "../../features/catalog/ProductDetails";
import {ToastContainer} from "react-toastify";
import 'react-toastify/dist/ReactToastify.css'
import ServerError from "../errors/ServerError";
import BasketPage from "../../features/basket/BasketPage";
import LoadingComponent from "./LoadingComponent";
import CheckoutPage from "../../features/checkout/CheckoutPage";
import {fetchBasketAsync, setBasket} from "../../features/basket/basketSlice";
import Login from "../../features/account/Login";
import Register from "../../features/account/Register";
import {useAppDispatch} from "../store/configureStore";
import {fetchCurrentUser} from "../../features/account/accountSlice";
import CheckoutWrapper from "../../features/checkout/CheckoutWrapper";
import {Inventory} from "@mui/icons-material";
import Orders from "../../features/orders/Orders";
import NotFound from "../errors/NotFound";
import PrivateRoute from "./PrivateRoute";



function App() {
    const [darkMode, setDarkMode] = useState(false);
    const paletteType = darkMode ? 'dark' : 'light';
    const [loading, setLoading] = useState(true);
    const dispatch = useAppDispatch();

    const theme = createTheme({
        palette: {
            mode: paletteType,
            background: {default: paletteType === 'light' ? '#eaeaea' : '#121212'}
        }
    })

    const initApp = useCallback(async () => {
        try {
            await dispatch(fetchCurrentUser());
            await dispatch(fetchBasketAsync());
        } catch (e) {
            console.log(e)
        }
    }, [dispatch])


    useEffect(()=>{
       initApp().then(()=> setLoading(false))
    },[initApp])



    function handleThemeChange() {
        setDarkMode(!darkMode);
    }

    if(loading) return <LoadingComponent message={'Initializing app'}/>

    return (
        <>
            <ThemeProvider theme={theme}>
                <ToastContainer position={"bottom-right"}  hideProgressBar/>
                <CssBaseline/>
                <Header darkMode={darkMode} handleThemeChange={handleThemeChange}/>
                <Container>
                    <Routes>
                        <Route path={'/*'} element={<HomePage/>}/>
                        <Route path={'/catalog/*'} element={<Catalog/>}/>
                        <Route path={'/catalog/:id'} element={<ProductDetails/>}/>
                        <Route path={'/about'} element={<AboutPage/>}/>
                        <Route path={'/contact'} element={<ContactPage/>}/>
                        <Route path={'/server-error'} element={<ServerError/>}/>
                        <Route path={'/basket'} element={<BasketPage/>}/>
                        <Route path={'/checkout'} element={<CheckoutPage/>}/>
                        <Route path={'/login'} element={<Login/>}/>
                        <Route path={'/register'} element={<Register/>}/>
                        <Route path={'/inventory'} element={ <PrivateRoute roles={['Admin']}/>} >
                            <Route path={'/inventory'} element={<Inventory/>}/>
                        </Route>
                        <Route path={'/orders'} element={ <PrivateRoute />} >
                            <Route path={'/orders'} element={<Orders/>}/>
                        </Route>
                        <Route path={'/checkout'} element={ <PrivateRoute />} >
                            <Route path={'/checkout'} element={<CheckoutWrapper/>}/>
                        </Route>
                        <Route element={<NotFound/>} />

                    </Routes>
                </Container>
            </ThemeProvider>
        </>
    );
}

export default App;
