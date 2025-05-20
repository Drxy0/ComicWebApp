// question: one big enum file or one file for each?
// Msm da sve ovo ide u tandemu, koristi se za display u comic/:id ruti
// Al ako bih implementirao search feature, nzm dal da razdvojim, kapiram ne
// Koristi se type umjesto enum, parsiracu ga na backu na enum
export type PublicationStatus = 
  | 'Ongoing' 
  | 'Completed' 
  | 'Hiatus' 
  | 'Cancelled';

export type Theme = 
  | 'Aliens' | 'Animals' | 'Cooking' | 'Demons' 
  | 'Ghosts' | 'Mafia' | 'MartialArts' | 'Military' 
  | 'Monsters' | 'Music' | 'PostApocalyptic' | 'Reincarnation' 
  | 'School' | 'Supernatural' | 'Survival' | 'Urban' 
  | 'Zombies';

export type Genre = 
  | 'Action' | 'Adventure' | 'Comedy' | 'Crime' 
  | 'Cyberpunk' | 'Drama' | 'Fantasy' | 'Historical' 
  | 'Horror' | 'Isekai' | 'Mystery' | 'Philosophical' 
  | 'Psychological' | 'Romance' | 'ScienceFiction' | 'Seinen' 
  | 'Shonen' | 'Shojo' | 'Superhero' | 'SliceOfLife' 
  | 'Sports' | 'Thriller' | 'Tragedy' | 'Western';