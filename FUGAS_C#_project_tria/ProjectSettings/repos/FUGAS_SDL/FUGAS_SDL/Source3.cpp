//#include <SDL.h>
//#include <SDL_image.h>
//#include <ctime>
//#include <iostream>
//#include <string>
//#include <SDL_ttf.h>
//
//#define screen_height 294
//#define screen_width 588
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
//		SDL_SetColorKey(surface, SDL_TRUE, SDL_MapRGB(surface->format, 255, 0xFF, 0xFF));
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
//	//вивід на екран картинки
//	void render(SDL_Renderer* renderer, int x, int y, SDL_Rect* sprite_part=NULL)
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
//
//		//та вивід на екран
//		SDL_RenderCopy(renderer, texture, sprite_part, &renderer_squad);
//	}
//
//	void render_hero(SDL_Renderer* renderer, int x, int y, SDL_Rect* sprite_part = NULL)
//	{
//		//формуємо квадрат що відповідає розташуванню текстури на екрані
//		SDL_Rect renderer_squad = { x,y,width,height };
//		if (sprite_part != NULL)
//		{
//			renderer_squad.w = 50;
//			renderer_squad.h = 75;
//		}
//		//зміна позиції зображення
//		pos_y = y;
//		pos_x = x;
//
//		//та вивід на екран
//		SDL_RenderCopy(renderer, texture, sprite_part, &renderer_squad);
//
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
//int main(int arc, char** argv)
//{
//	srand(time(NULL));
//	SDL_Init(SDL_INIT_VIDEO);
//	IMG_Init(IMG_INIT_PNG);
//	TTF_Init();
//
//	//створюємо змінні для роботи програми
//	SDL_Window* main_window = SDL_CreateWindow("task", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, screen_width, screen_height, SDL_WINDOW_SHOWN);
//	SDL_Renderer* main_renderer = SDL_CreateRenderer(main_window, -1, SDL_RENDERER_ACCELERATED | SDL_RENDERER_PRESENTVSYNC);
//
//	my_texture texture_image;//текстура заднього фону
//
//	//завантажуємо текстуру 
//	texture_image.load_from_file("background.jpg", main_renderer);
//
//	my_texture hero_animation[8];//анімація героя
//	for(int i=0;i<8;++i)
//		hero_animation[i].load_from_file(std::to_string(i)+".png", main_renderer);//завантажуємо зображенння для анімації
//
//	SDL_Rect hero_part = { 50,115,180,260 };//частина зображення, яку будемо виводити на екран
//
//	//кількість кадрів від першого кадру анімації героя
//	int counter = 0;
//
//	//частина заднього фону яку виводимо
//	SDL_Rect sprite_part_first_background = { 0,0,texture_image.get_width(),texture_image.get_height() };
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
//		}
//		//виводимо на екран текстури доки не вийдемо з програми
//		SDL_SetRenderDrawColor(main_renderer, 255, 255, 255, 255);
//		SDL_RenderClear(main_renderer);
//
//		++counter;
//
//		//якщо задній фон не видимий на екрані то змінюємо його позицію
//		if (sprite_part_first_background.x == screen_width)
//			sprite_part_first_background = { 0,0,texture_image.get_width(),texture_image.get_height() };
//
//		//виводимо задній фон
//		texture_image.render(main_renderer,0, 0, &sprite_part_first_background);
//		texture_image.render(main_renderer, sprite_part_first_background.w, 0, NULL);
//
//		//якщо показали останній кадр анімації, обнуляємо лічильник
//		if (counter == 104)
//			counter = 0;
//
//		//виводимо анімацію героя
//		hero_animation[counter/13].render_hero(main_renderer, 100,200, &hero_part);
//
//		//змінюємо частина заднього фону яку виводимо
//		sprite_part_first_background.x += 1;
//		sprite_part_first_background.w -= 1;
//		SDL_RenderPresent(main_renderer);
//
//	}
//
//	//звільнення пам'яті
//	texture_image.free();
//	for (int i = 0; i < 8; ++i)
//		hero_animation[i].free();
//
//	SDL_DestroyRenderer(main_renderer);
//	SDL_DestroyWindow(main_window);
//
//	main_renderer = NULL;
//	main_window = NULL;
//	SDL_Quit();
//	IMG_Quit();
//	TTF_Quit();
//
//	return 0;
//}