__kernel void square(__global float* arr){
	int i = get_global_id(0);
	arr[i] = arr[i] * arr[i];
}

__kernel void multiply(__global float* arr, float multiplier){
	int i = get_global_id(0);
	arr[i] = arr[i] * multiplier;
}

__kernel void negative_uncoalesced(__global uchar* image, int w, int h, int padding, __global uchar* imageOut){
	int x = get_global_id(0);
	int y = get_global_id(1);
	
	int idx = y * (w*3 + padding) + x*3;
	
	if((x < w) && (y < h)){// check if x and y are valid image coordinates
		imageOut[idx] = 255 - image[idx];
		imageOut[idx+1] = 255 - image[idx+1];
		imageOut[idx+2] = 255 - image[idx+2];
	}
}

__kernel void negative_coalesced(__global uchar4* image, int w, int h, int padding, __global uchar4* imageOut){
	int i = get_global_id(0)+get_global_id(1)*get_global_size(0);
	int idx = i + (i/w) * padding/4;
	
	uchar4 neg = (uchar4)(255,255,255,0);
	
	if (i < w*h) {
		imageOut[idx] = neg - image[idx];
	}
}
  
__constant sampler_t sampler = CLK_NORMALIZED_COORDS_FALSE | //Natural coordinates
							   CLK_ADDRESS_CLAMP_TO_EDGE | //Clamp to zeros
							   CLK_FILTER_NEAREST;
__kernel void negative_image2D(__read_only image2d_t image, __write_only image2d_t imageOut, int w, int h){
	
							   
	int iX = get_global_id(0);
	int iY = get_global_id(1);
	
	uint4 neg = (uint4) (255, 255, 255, 255);
	
	if ((iX >= 0)&&(iX < w) && (iY >= 0)&&(iY < h)) {
		uint4 pixelV = read_imageui( image, sampler, (int2)(iX,iY));
		write_imageui( imageOut, (int2)(iX,iY) , neg - pixelV );
	}
	
}

__kernel void teste(__global float* arr){
	
	int g = get_global_id(0) ;
	int l = get_global_id(1);
	int i = get_global_id(0) + get_local_id(1) * get_num_groups(0);
	if(g == 35 && l == 151){
		arr[i] = i;
	}
}

__kernel void RedDetection(__global uchar* imageRed, int w, int h, int padding, __global uchar* imageOutRed){
	int x = get_global_id(0);
	int y = get_global_id(1);
	
	float var_R, var_G, var_B, del_B, del_G, del_R;
	float var_Min, var_Max, del_Max;
	float V, H, S, B, G, R, var_1, var_2, var_3, var_b, var_g, var_r, var_i, var_h;
	
	if((x < w) && (y < h)){// check if x and y are valid image coordinates
		
		int idx = y * (w*3 + padding) + x*3;
		
		var_B = ( imageRed[idx] / 255.0 );                    
		var_G = ( imageRed[idx + 1] / 255.0 );
		var_R = ( imageRed[idx+ 2 ] / 255.0 );
		
		var_Max = max(max(var_R, var_G), var_B);
		var_Min = min(min(var_R, var_G), var_B);
 
        del_Max = var_Max - var_Min;            //Delta RGB value
 
        V = var_Max;
 
        if (del_Max == 0){                     //This is a gray, no chroma...
            H = 0;                                //HSV results from 0 to 1
            S = 0;
        }
        else                                    //Chromatic data...
        {
            if (var_Max == var_R)
                H = (((var_G - var_B) / del_Max) *60);
            else if (var_Max == var_G)
                H = ((var_B - var_R) / del_Max) *60+ 120;
            else 
                H = ((var_R - var_G) / del_Max)*60 + 240;
 
            if (H < 0)
                H = H + 360;
			H = H/2;
            
            S = del_Max / var_Max *255;
			V= V*255;
        }
	
		if((H >= 160 || H < 10)  && (S > 100) && (V >= 50))
			{
				H = 255;
				S = 0;
				V = 255;
		    } else {
				H = 0;
				S = 0;
				V = 0;
			}

			if ( S == 0 )                       //HSV from 0 to 1
			{
			   R = V * 255;
			   G = V * 255;
			   B = V * 255;
			}
			else
			{
			   var_h = H * 6;
			   if ( var_h == 6 ) var_h = 0;      //H must be < 1
			   var_i = (int) var_h;             //Or ... var_i = floor( var_h )
			   var_1 = V * ( 1 - S );
			   var_2 = V * ( 1 - S * ( var_h - var_i ) );
			   var_3 = V * ( 1 - S * ( 1 - ( var_h - var_i ) ) );

			   if      ( var_i == 0 ) { var_r = V     ; var_g = var_3 ; var_b = var_1; }
			   else if ( var_i == 1 ) { var_r = var_2 ; var_g = V     ; var_b = var_1; }
			   else if ( var_i == 2 ) { var_r = var_1 ; var_g = V     ; var_b = var_3; }
			   else if ( var_i == 3 ) { var_r = var_1 ; var_g = var_2 ; var_b = V;     }
			   else if ( var_i == 4 ) { var_r = var_3 ; var_g = var_1 ; var_b = V;     }
			   else                   { var_r = V     ; var_g = var_1 ; var_b = var_2; }

			   R = var_r * 255;                  //RGB results from 0 to 255
			   G = var_g * 255;
			   B = var_b * 255;
			}
			
			imageOutRed[idx] = (uchar) B;
			imageOutRed[idx + 1] = (uchar) G;
			imageOutRed[idx + 2] = (uchar) R;
	}	
}

__kernel void BlueDetection(__global uchar* imageBlue, int w, int h, int padding, __global uchar* imageOutBlue){
	int x = get_global_id(0);
	int y = get_global_id(1);
	
	float var_R, var_G, var_B, del_B, del_G, del_R;
	float var_Min, var_Max, del_Max;
	float V, H, S, B, G, R, var_1, var_2, var_3, var_b, var_g, var_r, var_i, var_h;
	
	if((x < w) && (y < h)){// check if x and y are valid image coordinates
		
		int idx = y * (w*3 + padding) + x*3;
		
		var_B = ( imageBlue[idx] / 255.0 );                    
		var_G = ( imageBlue[idx + 1] / 255.0 );
		var_R = ( imageBlue[idx+ 2 ] / 255.0 );
		
		var_Max = max(max(var_R, var_G), var_B);
		var_Min = min(min(var_R, var_G), var_B);
 
        del_Max = var_Max - var_Min;            //Delta RGB value
 
        V = var_Max;
 
        if (del_Max == 0)                     //This is a gray, no chroma...
        {
            H = 0;                                //HSV results from 0 to 1
            S = 0;
        }
        else                                    //Chromatic data...
        {
            if (var_Max == var_R)
                H = (((var_G - var_B) / del_Max) * 60);
            else if (var_Max == var_G)
                H = ((var_B - var_R) / del_Max) * 60+ 120;
            else 
                H = ((var_R - var_G) / del_Max)* 60 + 240;
 
            if (H < 0)
                H = H + 360;
			H = H/2;
            
            S = del_Max / var_Max * 255;
			V= V * 255;
        }
		
		if((H >= 103.0 && H <= 160.0) && (S >= 30.0) && (V >= 50.0)){								
				H = 255;
				S = 0;
				V = 255;
            } else {
				H = 0;
				S = 0;
				V = 0;
			}
			
			if ( S == 0 )                       //HSV from 0 to 1
			{
			   R = V * 255;
			   G = V * 255;
			   B = V * 255;
			}
			else
			{
			   var_h = H * 6;
			   if ( var_h == 6 ) var_h = 0;      //H must be < 1
			   var_i = (int) var_h;             //Or ... var_i = floor( var_h )
			   var_1 = V * ( 1 - S );
			   var_2 = V * ( 1 - S * ( var_h - var_i ) );
			   var_3 = V * ( 1 - S * ( 1 - ( var_h - var_i ) ) );

			   if      ( var_i == 0 ) { var_r = V     ; var_g = var_3 ; var_b = var_1; }
			   else if ( var_i == 1 ) { var_r = var_2 ; var_g = V     ; var_b = var_1; }
			   else if ( var_i == 2 ) { var_r = var_1 ; var_g = V     ; var_b = var_3; }
			   else if ( var_i == 3 ) { var_r = var_1 ; var_g = var_2 ; var_b = V;     }
			   else if ( var_i == 4 ) { var_r = var_3 ; var_g = var_1 ; var_b = V;     }
			   else                   { var_r = V     ; var_g = var_1 ; var_b = var_2; }

			   R = var_r * 255;                  //RGB results from 0 to 255
			   G = var_g * 255;
			   B = var_b * 255;
			}
			
			imageOutBlue[idx] = (uchar) B;
			imageOutBlue[idx + 1] = (uchar) G;
			imageOutBlue[idx + 2] = (uchar) R;
	}	
}

__kernel void erode(__global uchar* src, int w, int h, int padding, __global uchar* dst){
	int x = get_global_id(0);
	int y = get_global_id(1);
	
	int idx_temp, idx;
	int total = 0;
	
	if((x < w) && (y < h)){// check if x and y are valid image coordinates
		
		idx = y * (w*3 + padding) + x*3;
		
		idx_temp = (y - 1 < 0 ? -(y - 1) : ((y - 1) > (h - 1) ? (y + 1) : (y - 1))) * (w*3 + padding) + (x - 1 < 0 ? -(x - 1) : ((x - 1) > (w - 1) ? (x + 1) : (x - 1)))*3;
		total += src[idx_temp];
		idx_temp = (y < 0 ? -(y) : ((y) > (h - 1) ? (y) : (y))) * (w*3 + padding) + (x - 1 < 0 ? -(x - 1) : ((x - 1) > (w - 1) ? (x + 1) : (x - 1)))*3;
		total += src[idx_temp];
		idx_temp = (y + 1 < 0 ? -(y + 1) : ((y + 1) > (h - 1) ? (y - 1) : (y + 1))) * (w*3 + padding) + (x - 1 < 0 ? -(x - 1) : ((x - 1) > (w - 1) ? (x + 1) : (x - 1)))*3;
		total += src[idx_temp];
		idx_temp = (y - 1 < 0 ? -(y - 1) : ((y - 1) > (h - 1) ? (y + 1) : (y - 1))) * (w*3 + padding) + (x < 0 ? -(x) : ((x) > (w - 1) ? (x) : (x)))*3;
		total += src[idx_temp];
		idx_temp = (y < 0 ? -(y) : ((y) > (h - 1) ? (y) : (y))) * (w*3 + padding) + (x < 0 ? -(x) : ((x) > (w - 1) ? (x) : (x)))*3;
		total += src[idx_temp];
		idx_temp = (y + 1 < 0 ? -(y + 1) : ((y + 1) > (h - 1) ? (y - 1) : (y + 1))) * (w*3 + padding) + (x < 0 ? -(x) : ((x) > (w - 1) ? (x) : (x)))*3;
		total += src[idx_temp];
		idx_temp = (y - 1 < 0 ? -(y - 1) : ((y - 1) > (h - 1) ? (y + 1) : (y - 1))) * (w*3 + padding) + (x + 1 < 0 ? -(x + 1) : ((x + 1) > (w - 1) ? (x - 1) : (x + 1)))*3;
		total += src[idx_temp];
		idx_temp = (y < 0 ? -(y) : ((y) > (h - 1) ? (y) : (y))) * (w*3 + padding) + (x + 1 < 0 ? -(x + 1) : ((x + 1) > (w - 1) ? (x - 1) : (x + 1)))*3;
		total += src[idx_temp];
		idx_temp = (y + 1 < 0 ? -(y + 1) : ((y + 1) > (h - 1) ? (y - 1) : (y + 1))) * (w*3 + padding) + (x + 1 < 0 ? -(x + 1) : ((x + 1) > (w - 1) ? (x - 1) : (x + 1)))*3;
		total += src[idx_temp];
		
		if(total >= 2290){ //tecnicamente é 2295
			dst[idx] = 255; //Blue
			dst[idx + 1] = 255; //Green
			dst[idx + 2] = 255; //Red
		} else {
			dst[idx] = 0; //Blue
			dst[idx + 1] = 0; //Green
			dst[idx + 2] = 0; //Red
		}
	}
}

__kernel void dilate(__global uchar* src, int w, int h, int padding, __global uchar* dst){
	int x = get_global_id(0);
	int y = get_global_id(1);
	
	int idx_temp, idx;
	int total = 0;
	
	if((x < w) && (y < h)){// check if x and y are valid image coordinates
		
		idx = y * (w*3 + padding) + x*3;

		idx_temp = (y - 1 < 0 ? -(y - 1) : ((y - 1) > (h - 1) ? (y + 1) : (y - 1))) * (w*3 + padding) + (x - 1 < 0 ? -(x - 1) : ((x - 1) > (w - 1) ? (x + 1) : (x - 1)))*3;
		total += src[idx_temp];
		idx_temp = (y < 0 ? -(y) : ((y) > (h - 1) ? (y) : (y))) * (w*3 + padding) + (x - 1 < 0 ? -(x - 1) : ((x - 1) > (w - 1) ? (x + 1) : (x - 1)))*3;
		total += src[idx_temp];
		idx_temp = (y + 1 < 0 ? -(y + 1) : ((y + 1) > (h - 1) ? (y - 1) : (y + 1))) * (w*3 + padding) + (x - 1 < 0 ? -(x - 1) : ((x - 1) > (w - 1) ? (x + 1) : (x - 1)))*3;
		total += src[idx_temp];
		idx_temp = (y - 1 < 0 ? -(y - 1) : ((y - 1) > (h - 1) ? (y + 1) : (y - 1))) * (w*3 + padding) + (x < 0 ? -(x) : ((x) > (w - 1) ? (x) : (x)))*3;
		total += src[idx_temp];
		idx_temp = (y < 0 ? -(y) : ((y) > (h - 1) ? (y) : (y))) * (w*3 + padding) + (x < 0 ? -(x) : ((x) > (w - 1) ? (x) : (x)))*3;
		total += src[idx_temp];
		idx_temp = (y + 1 < 0 ? -(y + 1) : ((y + 1) > (h - 1) ? (y - 1) : (y + 1))) * (w*3 + padding) + (x < 0 ? -(x) : ((x) > (w - 1) ? (x) : (x)))*3;
		total += src[idx_temp];
		idx_temp = (y - 1 < 0 ? -(y - 1) : ((y - 1) > (h - 1) ? (y + 1) : (y - 1))) * (w*3 + padding) + (x + 1 < 0 ? -(x + 1) : ((x + 1) > (w - 1) ? (x - 1) : (x + 1)))*3;
		total += src[idx_temp];
		idx_temp = (y < 0 ? -(y) : ((y) > (h - 1) ? (y) : (y))) * (w*3 + padding) + (x + 1 < 0 ? -(x + 1) : ((x + 1) > (w - 1) ? (x - 1) : (x + 1)))*3;
		total += src[idx_temp];
		idx_temp = (y + 1 < 0 ? -(y + 1) : ((y + 1) > (h - 1) ? (y - 1) : (y + 1))) * (w*3 + padding) + (x + 1 < 0 ? -(x + 1) : ((x + 1) > (w - 1) ? (x - 1) : (x + 1)))*3;
		total += src[idx_temp];

		if(total >= 250){ //tecnicamente é 255
			dst[idx] = 255; //Blue
			dst[idx + 1] = 255; //Green
			dst[idx + 2] = 255; //Red
		} else {
			dst[idx] = 0; //Blue
			dst[idx + 1] = 0; //Green
			dst[idx + 2] = 0; //Red
		}
	}
}

__kernel void histogram(__global uchar* src, int w, int h, int padding, __global int *hist, int vertical){
	int x = get_global_id(0);
	int y = get_global_id(1);
	
	if((x < w) && (y < h)){// check if x and y are valid image coordinates
		
		int idx = y * (w*3 + padding) + x*3;
		
		if(vertical == 1){
			if(src[idx] == 255){
				atom_inc(&hist[x]);
			}
		}
		if(vertical == 0){
			if(src[idx] == 255){
				atom_inc(&hist[y]);
			}
		}
	}
}

__kernel void matchTemplate( __global uchar* i, __global uchar* t, int w, int h, int padding, float factor_img, float factor_tmp, int sum_I, int sum_T, __global int *nominator, __global int *sum_T_2, __global int *sum_I_2){
    int x = get_global_id(0);
    int y = get_global_id(1);

    if((x < w) && (y<h)){
		int idx = y * (w*3 + padding) + x*3;
		
        atom_add(&nominator[0],(int)(((float)((i[idx]+i[idx+1]+i[idx+2])/3) - factor_img * (float)sum_I) * (((float)(t[idx]+t[idx+1]+t[idx+2])/3) - factor_tmp * (float)sum_T)));
		atom_add(&sum_T_2[0],(int)((((float)(t[idx]+t[idx+1]+t[idx+2])/3) - factor_tmp * (float)sum_T) * (((float)(t[idx]+t[idx+1]+t[idx+2])/3) - factor_tmp * (float)sum_T)));
		atom_add(&sum_I_2[0],(int)((((float)(i[idx]+i[idx+1]+i[idx+2])/3) - factor_img * (float)sum_I) * (((float)(i[idx]+i[idx+1]+i[idx+2])/3) - factor_img * (float)sum_I)));
    }
}

__kernel void sumImgPixels(__global uchar* src, int w, int h, int padding, __global int *sum){
	int x = get_global_id(0);
    int y = get_global_id(1);

    if((x < w) && (y<h)){
		int idx = y * (w*3 + padding) + x*3;
		
		atom_add(&sum[0], (int)((float)src[idx] + (float)src[idx+1] + (float)src[idx+2])/3);
	}
}

__kernel void sum2ImgPixels(__global uchar* src, int w, int h, int padding, float factor, int sum, __global int *sum2){
	int x = get_global_id(0);
    int y = get_global_id(1);

    if((x < w) && (y<h)){
		int idx = y * (w*3 + padding) + x*3;
		
		atom_add(sum2,(int)((((float)(src[idx]+src[idx+1]+src[idx+2])/3) - factor * (float)sum) * (((float)(src[idx]+src[idx+1]+src[idx+2])/3) - factor * (float)sum)));
	}
}