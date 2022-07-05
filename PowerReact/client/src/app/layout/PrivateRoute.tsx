import { RouteProps } from "react-router";
import { toast } from "react-toastify";
import { useAppSelector } from "../store/configureStore";
import * as React from "react";
import { Navigate, Outlet } from 'react-router-dom';

interface Props  extends  RouteProps{
    roles?: string[];
}




export default function PrivateRoute({ roles,  ...rest }:Props) {
        const { user } = useAppSelector(state => state.account);
        if (!user) {
            return <Navigate to={"/login"}/>
        }

        if (roles && !roles?.some(r => user!.roles?.includes(r))) {
            toast.error('Not authorised to access this area');
            return <Navigate to={"/login"}/>
        }

        return (
            <Outlet /> // goes to the child component in the <Route> that it was called from
        )
}
