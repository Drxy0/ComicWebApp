import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'languageFlag'
})
export class LanguageFlagPipe implements PipeTransform {
    private languageFlagsDictionary: { [key: string]: { src: string; title: string; alt: string } } =  {
        en: { src: 'assets/flags/gb.svg', title: 'English', alt: 'English flag icon' },
        jp: { src: 'assets/flags/jp.svg', title: 'Japanese', alt: 'Japanese flag icon'},
        fr: { src: 'assets/flags/fr.svg', title: 'French', alt: 'French flag icon'},
    }
    
    transform(languageCode: string): { src: string; title: string; alt: string } | null {
        const normalisedCode = languageCode.toLowerCase();
        return this.languageFlagsDictionary[normalisedCode] || null;
    }
}