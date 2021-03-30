//#include <SDL.h>
//#include <SDL_image.h>
//#include <ctime>
//#include <iostream>
//#include <SDL_ttf.h>
//
//#define screen_height 800
//#define screen_width 800
//
////клас текстури
//class my_texture
//{
//	SDL_Texture* texture;//сама текстура
//	int width;//розміри
//	int height;
//	int pos_x;//поточна позиція на екрані
//	int pos_y;
//
//public:
//	//конструктор
//	my_texture()//
//	{
//		texture = NULL;
//		width = 0;
//		height = 0;
//	}
//
//	//завантаження текстури з файлу
//	void load_from_file(std::string file, SDL_Renderer* renderer)
//	{
//		//звільнення раніше виділеної пам'яті
//		free();
//
//		//завантаження картинки та видалення заднього фону
//		SDL_Surface* surface = IMG_Load(file.c_str());
//		SDL_SetColorKey(surface, SDL_TRUE, SDL_MapRGB(surface->format, 0, 0xFF, 0xFF));
//
//		//присвоєння даних для полів класу
//		texture = SDL_CreateTextureFromSurface(renderer, surface);
//		width = surface->w;
//		height = surface->h;
//
//		//зміна параметрів текстури
//		set_blend_mode(SDL_BLENDMODE_BLEND);
//
//		//звільнення пам'яті
//		SDL_FreeSurface(surface);
//	}
//
//	//завантаження текстури з тексту
//	void load_from_text(std::string text, TTF_Font* font, SDL_Renderer* renderer, SDL_Color text_color)
//	{
//		//звільнення раніше виділеної пам'яті
//		free();
//
//		//завантаження картинки та видалення заднього фону
//		SDL_Surface* surface = TTF_RenderText_Solid(font, text.c_str(), text_color);
//		SDL_SetColorKey(surface, SDL_TRUE, SDL_MapRGB(surface->format, 0, 0xFF, 0xFF));
//
//		//присвоєння даних для полів класу
//		texture = SDL_CreateTextureFromSurface(renderer, surface);
//		width = surface->w;
//		height = surface->h;
//
//
//		//звільнення пам'яті
//		SDL_FreeSurface(surface);
//	}
//
//	//звільнення пам'яті з-під текстури
//	void free()
//	{
//		//якщо текстура не порожня то звільняємо пам'ять
//		if (texture != NULL)
//		{
//			SDL_DestroyTexture(texture);
//			texture = NULL;
//			width = 0;
//			height = 0;
//		}
//	}
//
//	//сеттер для прозорості картинки
//	void set_alpha(int a)
//	{
//		SDL_SetTextureAlphaMod(texture, a);
//	}
//
//	//зміна параметрів картинки
//	void set_blend_mode(SDL_BlendMode mode)
//	{
//		SDL_SetTextureBlendMode(texture, mode);
//	}
//
//	//вивід на екран картинки
//	void render(SDL_Renderer* renderer, int x, int y, SDL_Rect* sprite_part = NULL)
//	{
//		//формуємо квадрат що відповідає розташуванню текстури на екрані
//		SDL_Rect renderer_squad = { x,y,width,height };
//		if (sprite_part != NULL)
//		{
//			renderer_squad.w = sprite_part->w;
//			renderer_squad.h = sprite_part->h;
//		}
//		//зміна позиції зображення
//		pos_y = y;
//		pos_x = x;
//		//та вивід на екран
//		SDL_RenderCopy(renderer, texture, sprite_part, &renderer_squad);
//	}
//
//	//присоєння кольору для текстури
//	void set_color(int r, int g, int b)
//	{
//		SDL_SetTextureColorMod(this->texture, r, g, b);
//	}
//
//	//отримання поточних позицій спрайта
//	int get_position_x()
//	{
//		return pos_x;
//	}
//
//	int get_position_y()
//	{
//		return pos_y;
//	}
//
//	//геттери для розміру текстури
//	int get_height()
//	{
//		return height;
//	}
//
//	int get_width()
//	{
//		return width;
//	}
//
//	//деструктор
//	~my_texture()
//	{
//		free();//очищуємо пам'ять
//	}
//};
//
////Генерація випадкової позиції на екрані для текстури
//SDL_Rect generate_position(int img_height,int img_width)
//{
//	int center_x = rand()% (screen_width- img_width) + img_width/2;
//	int center_y = rand() % (screen_height - img_height) + img_height / 2;
//	return { center_x- img_width / 2,center_y - img_height / 2,center_x + img_width / 2,center_y + img_height / 2 };
//}
//
////перевірка чи позиція мишки знаходиться на зображенні
//bool check_positions(my_texture *img)
//{
//	int x, y;
//	SDL_GetMouseState(&x,&y);
//	return img->get_position_x() <= x && img->get_position_x() + img->get_width() >= x && img->get_position_y() <= y && img->get_position_y() + img->get_height() >= y;
//}
//
//int main(int arc,char**argv)
//{
//	srand(time(NULL));
//	SDL_Init(SDL_INIT_VIDEO|SDL_INIT_AUDIO);
//	IMG_Init(IMG_INIT_PNG);
//
//	//створюємо змінні для роботи програми
//	SDL_Window* main_window = SDL_CreateWindow("task", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, screen_width, screen_height, SDL_WINDOW_SHOWN| SDL_WINDOW_RESIZABLE);
//	SDL_Renderer* main_renderer = SDL_CreateRenderer(main_window, -1, SDL_RENDERER_ACCELERATED | SDL_RENDERER_PRESENTVSYNC);
//
//	//створюємо текстуру
//	my_texture texture;
//	texture.load_from_file("purebackground.png", main_renderer);
//	
//	//генеруємо позицію для текстури
//	SDL_Rect pos = generate_position(texture.get_height(), texture.get_width());
//
//	//доки не закриємо програму
//	SDL_Event events;
//	bool exit = false;
//	while (!exit)
//	{
//		while (SDL_PollEvent(&events) != 0)
//		{
//			if (events.type == SDL_QUIT)
//			{
//				exit = true;
//				break;
//			}
//			else
//				//якщо натиснуто на мишкою на зображення, генеруємо нову позицію для зображення
//				if (events.type == SDL_MOUSEBUTTONUP&& check_positions(&texture))
//						pos = generate_position(texture.get_height(), texture.get_width());	
//		}
//
//		//промальовуємо текстуру
//		SDL_SetRenderDrawColor(main_renderer, 0, 255, 255, 255);
//		SDL_RenderClear(main_renderer);
//		
//		texture.render(main_renderer, pos.x,pos.y);
//		SDL_RenderPresent(main_renderer);
//	}
//
//	//звільнення пам'яті
//	texture.free();
//	SDL_DestroyRenderer(main_renderer);
//	SDL_DestroyWindow(main_window);
//	main_renderer = NULL;
//	main_window = NULL;
//	SDL_Quit();
//	IMG_Quit();
//	return 0;
//}