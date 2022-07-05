import {
    Avatar, Button,
    Card,
    CardActions,
    CardContent, CardHeader,
    CardMedia,
    Typography
} from "@mui/material";
import {Link} from "react-router-dom";
import {LoadingButton} from "@mui/lab";
import {addBasketItemAsync} from "../basket/basketSlice";
import {Product} from "../../app/models/product/Product";
import {useAppDispatch, useAppSelector} from "../../app/store/configureStore";

interface Props {
    product: Product,
}

export default function ProductCard({product}: Props) {
    const {status} = useAppSelector(state => state.basket);
    const dispatch = useAppDispatch();


    return (
        <Card>
            <CardHeader avatar={
                <Avatar sx={{bgcolor: 'secondary.main'}}>
                    {product.name.charAt(0).toUpperCase()}</Avatar>
            }
                        title={[product.name]}
                        titleTypographyProps={{
                            sx: {fontWeight: 'bold', color: 'primary.main'}
                        }}
            />
            <CardMedia
                component="img"
                height="140"
                sx={{backgroundSize: 'contain', bgcolor: 'primary.light'}}
                image={product.pictureUrl}
                title={product.name}
            />
            <CardContent>
                <Typography gutterBottom color={'secondary'} variant="h5" component="div">
                    €{(product.price /100).toFixed(2)}
                </Typography>
                <Typography variant="body2" color="text.secondary">
                    {product.brand} / {product.type}
                </Typography>
            </CardContent>
            <CardActions>
                <LoadingButton loading={status === ('pendingAddItem' + product.id)}
                               onClick={()=>dispatch(addBasketItemAsync({productId: product.id}))}
                               size={'small'}>Add to cart</LoadingButton>
                <Button component={Link} to={`/catalog/${product.id}`} size="small">View</Button>
            </CardActions>
        </Card>
    )
}


