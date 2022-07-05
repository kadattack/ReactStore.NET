import {Basket, BasketItem} from "../app/models/Basket";
import {createContext, PropsWithChildren, useContext, useState} from "react";
import {Simulate} from "react-dom/test-utils";
const _ = require("lodash.clonedeep");


// like a service

interface StoreContextValue {
    basket: Basket | null;
    setBasket: (basket: Basket) => void;
    removeItem: (productId: number, quantity: number) => void;
}

export const StoreContext = createContext<StoreContextValue | undefined>(undefined)

// custom react hook
export function useStoreContext() {
    const context = useContext(StoreContext);

    if (context === undefined) {
        throw Error('You are not inside the provider for context')
    }
    return context;
}


export function StoreProvider({children}: PropsWithChildren<any>) {
    const [basket, setBasket] = useState<Basket | null>(null);
    function removeItem(productId: number, quantity: number) {
        if (!basket) return;
        const items = [...basket.items]
        const itemIndex = items.findIndex((i:BasketItem) => i.productId === productId)
        if (itemIndex >= 0) {
            items[itemIndex].quantity -= quantity;
            if (items[itemIndex].quantity <= 0)
                items.splice(itemIndex, 1)
            setBasket(prevState => {
                return {...prevState!, items}
            })

        }
    }

    return (
        <StoreContext.Provider value={{basket, setBasket, removeItem}}>
            {children}
        </StoreContext.Provider>
    )
}